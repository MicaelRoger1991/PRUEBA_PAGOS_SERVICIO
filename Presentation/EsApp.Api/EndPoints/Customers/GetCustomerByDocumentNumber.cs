using System;
using EsApp.Api.Abstractions;
using EsApp.Application.Customers;
using EsApp.Application.Customers.Request;
using EsApp.Application.Customers.Response;
using EsApp.CROSS.Shared.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Security.Claims;

namespace EsApp.Api.EndPoints.Customers;

public class GetCustomerByDocumentNumber : IEndPoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"{Tags.Customers}/GetCustomerByDocumentNumber", Handler)
            .RequireAuthorization()
            .Produces<CustomerResponse>(StatusCodes.Status200OK)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.Customers);
    }

    private async Task<IResult> Handler(
        ICustomersService customersService,
        HttpContext httpContext,
        [FromQuery] string documentNumber,
        IValidator<GetCustomerByDocumentNumberRequest> validator,
        CancellationToken cancellationToken)
    {
        var roleClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        if (string.IsNullOrWhiteSpace(roleClaim))
        {
            return Results.Unauthorized();
        }

        var role = ParseRole(roleClaim);
        if (role != "CAJA" && role != "PLATAFORMA" && role != "GERENTE")
        {
            return Results.Forbid();
        }

        var request = new GetCustomerByDocumentNumberRequest(documentNumber);
        var result = await customersService.GetCustomerByDocumentNumberAsync(request, validator, cancellationToken);
        if (result.IsFailure)
        {
            if (result.Error == EsApp.Domain.Customers.CustomersError.NotFound)
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
