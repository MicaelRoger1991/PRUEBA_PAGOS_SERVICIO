using EsApp.Application.ServiceProvider.Response;
using EsApp.CROSS.Shared.Abstractions;

namespace EsApp.Application.ServiceProvider;

public interface IServiceProviderService
{
    Task<Result<IReadOnlyList<ServiceProviderResponse>>> GetServiceProvidersAsync(CancellationToken cancellationToken);
}
