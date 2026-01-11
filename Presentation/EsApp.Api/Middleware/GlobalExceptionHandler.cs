using System.Text.Json;
using EsApp.CROSS.Shared.Abstractions;
using EsApp.CROSS.Shared.Abstractions.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace EsApp.Api.Middleware;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly EsApp.CROSS.Logger.ILogger _logger;

    public GlobalExceptionHandler(EsApp.CROSS.Logger.ILogger logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Añadimos un bloque para manejar específicamente las excepciones de validación.
        if (exception is ValidationException validationException)
        {
            // Creamos un diccionario con los errores para una respuesta clara.
            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(new { errors }, cancellationToken);

            return true; // Indicamos que la excepción ha sido manejada.
        }


        Error errorResponse;

        if (exception is ServiceException serviceExceptions)
        {
            errorResponse = new Error(serviceExceptions.Error!.Code, "Ha ocurrido un error inesperado, por favor intenta nuevamente.");
        }
        else
        {
            errorResponse = new Error("500", "Ha ocurrido un error inesperado, por favor intenta nuevamente.");
        }


        try
        {
            httpContext.Request.Body.Position = 0;
            var request = httpContext.Request.HasJsonContentType()
                        ? await httpContext.Request.ReadFromJsonAsync<ServiceRequest>(cancellationToken: default)
                        : null;

            if (request == null)
            {
                SaveToLogs(httpContext, exception, errorResponse);
                return await ReturnErrorResponse(httpContext, errorResponse);
            }
            SaveToLogs(httpContext, exception, errorResponse, request);
            return await ReturnErrorResponse(httpContext, errorResponse);
        }
        catch (Exception ex)
        {
            SaveToLogs(httpContext, ex, errorResponse);
            return await ReturnErrorResponse(httpContext, errorResponse);
        }
    }

    private void SaveToLogs(HttpContext context, Exception exception, Error error, ServiceRequest? serviceRequest = null)
    {
        var correlationId = $"{context.Request.Headers["CorrelationId"]}";

        _logger.Error(" | CorrelationId -> {0} | Path: {1} \nErrorMessage: {2} \nRequest: {3} \nResponse: {4} \nStackTrace: {5}",
            correlationId,
            context.Request.Path,
            exception.Message,
            serviceRequest is null ? "" : JsonSerializer.Serialize(serviceRequest),
            JsonSerializer.Serialize(error),
            exception.StackTrace!);
    }

    private static async ValueTask<bool> ReturnErrorResponse(HttpContext context, Error error)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(error)).ConfigureAwait(false);

        return true;
    }
}
