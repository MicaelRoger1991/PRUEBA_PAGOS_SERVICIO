using System;

namespace EsApp.Application.ServiceProvider.Response;

public record ServiceProviderResponse
(
    Guid serviceProviderId,
    string service,
    Guid currencyId,
    string currency
);
