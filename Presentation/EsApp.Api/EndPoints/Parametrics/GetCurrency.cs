using System;
using EsApp.Api.Abstractions;
using EsApp.Application.Parametrics;
using EsApp.Application.Parametrics.Response;
using EsApp.CROSS.Shared.Abstractions;

namespace EsApp.Api.EndPoints.Parametrics;

public class GetCurrency : IEndPoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"{Tags.Parametrics}/GetCurrency", Handler)
            .RequireAuthorization()
            .Produces<IReadOnlyList<CurrencyResponse>>(StatusCodes.Status200OK)
            .Produces<Error>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.Parametrics);
    }

    private async Task<IResult> Handler(
        ICurrencyService currencyService,
        CancellationToken cancellationToken)
    {
        var result = await currencyService.GetCurrenciesAsync(cancellationToken);
        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }
        return Results.Ok(result.Value);
    }
}
