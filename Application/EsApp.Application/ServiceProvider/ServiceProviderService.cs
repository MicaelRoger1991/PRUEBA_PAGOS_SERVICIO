using System;
using EsApp.Application.ServiceProvider.Response;
using EsApp.CROSS.Shared.Abstractions;
using EsApp.Domain.ServiceProvider;

namespace EsApp.Application.ServiceProvider;

public class ServiceProviderService : IServiceProviderService
{
    private readonly IServiceProviderRepository _serviceProviderRepository;

    public ServiceProviderService(IServiceProviderRepository serviceProviderRepository)
    {
        _serviceProviderRepository = serviceProviderRepository;
    }

    public async Task<Result<IReadOnlyList<ServiceProviderResponse>>> GetServiceProvidersAsync(CancellationToken cancellationToken)
    {
        var providers = await _serviceProviderRepository.GetActiveAsync();
        var result = providers
            .Select(item => new ServiceProviderResponse(
                item.serviceProviderId,
                item.service,
                item.currencyId,
                item.currency
            ))
            .ToList();

        return Result.Success<IReadOnlyList<ServiceProviderResponse>>(result);
    }
}
