using System;

namespace EsApp.Domain.ServiceProvider;

public interface IServiceProviderRepository
{
    Task<IReadOnlyList<ServiceProvider>> GetActiveAsync();
}
