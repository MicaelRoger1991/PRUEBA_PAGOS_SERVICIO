using System;

namespace EsApp.Application.Parametrics.Response;

public record CurrencyResponse
(
    Guid currencyId,
    string currency,
    string shortName
);
