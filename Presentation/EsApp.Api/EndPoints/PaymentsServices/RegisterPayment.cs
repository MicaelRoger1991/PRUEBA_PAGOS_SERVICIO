using System;
using System.Security.Claims;
using System.Text.Json;
using EsApp.Api.Abstractions;
using EsApp.Application.Payments;
using EsApp.Application.Payments.Request;
using EsApp.Application.Payments.Response;
using EsApp.CROSS.Shared.Abstractions;
using EsApp.Domain.Payments;
using FluentValidation;

namespace EsApp.Api.EndPoints.PaymentsServices;

public class RegisterPayment : IEndPoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("payments", Handler)
            .Produces<RegisterPaymentResponse>(StatusCodes.Status200OK)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.PaymentsServices);
    }

    private async Task<IResult> Handler(
        IPaymentsService paymentsService,
        HttpContext httpContext,
        RegisterPaymentRequest request,
        IValidator<RegisterPaymentRequest> validator,
        CancellationToken cancellationToken)
    {
        var roleClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        if (string.IsNullOrWhiteSpace(roleClaim))
        {
            return Results.Unauthorized();
        }

        var role = ParseRole(roleClaim);
        if (role != "CAJA")
        {
            return Results.Forbid();
        }

        var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var usersMasterId))
        {
            return Results.Unauthorized();
        }

        var result = await paymentsService.RegisterPaymentAsync(request, usersMasterId, validator, cancellationToken);
        if (result.IsFailure)
        {
            if (result.Error == PaymentsError.CustomerNotFound || result.Error == PaymentsError.ServiceProviderNotFound)
            {
                return Results.NotFound(result.Error);
            }
            return Results.BadRequest(result.Error);
        }

        return Results.Ok(result.Value);
    }

    private static string ParseRole(string roleClaim)
    {
        try
        {
            return JsonSerializer.Deserialize<string>(roleClaim) ?? roleClaim;
        }
        catch (JsonException)
        {
            return roleClaim;
        }
    }
}
