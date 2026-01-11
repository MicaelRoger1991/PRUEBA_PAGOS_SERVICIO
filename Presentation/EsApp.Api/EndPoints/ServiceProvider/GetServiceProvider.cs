using System;
using EsApp.Api.Abstractions;
using EsApp.Application.ServiceProvider;
using EsApp.Application.ServiceProvider.Response;
using EsApp.CROSS.Shared.Abstractions;

namespace EsApp.Api.EndPoints.ServiceProvider;

public class GetServiceProvider : IEndPoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"{Tags.ServiceProvider}/GetServiceProvider", Handler)
            .RequireAuthorization()
            .Produces<IReadOnlyList<ServiceProviderResponse>>(StatusCodes.Status200OK)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.ServiceProvider);
    }

    private async Task<IResult> Handler(
        IServiceProviderService serviceProviderService,
        CancellationToken cancellationToken)
    {
        var result = await serviceProviderService.GetServiceProvidersAsync(cancellationToken);
        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }
        return Results.Ok(result.Value);
    }
}
