using System;
using System.Security.Claims;
using System.Text.Json;
using EsApp.Api.Abstractions;
using EsApp.Application.Payments;
using EsApp.Application.Payments.Response;
using EsApp.CROSS.Shared.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EsApp.Api.EndPoints.PaymentsServices;

public class GetPayments : IEndPoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("payments", Handler)
            .Produces<IReadOnlyList<PaymentResponse>>(StatusCodes.Status200OK)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.PaymentsServices);
    }

    private async Task<IResult> Handler(
        IPaymentsService paymentsService,
        HttpContext httpContext,
        [FromQuery] string customerId,
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
        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Results.Unauthorized();
        }

        if (!Guid.TryParse(customerId, out var customerGuid))
        {
            return Results.BadRequest(new Error("Payments.InvalidCustomerId", "CustomerId no es válido."));
        }

        var result = await paymentsService.GetPaymentsByCustomerAsync(customerGuid, cancellationToken);
        if (result.IsFailure)
        {
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
