using System;
using EsApp.Domain.ServiceProvider;
using EsApp.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EsApp.Persistence.Repository;

public class ServiceProviderRepository : IServiceProviderRepository
{
    private readonly EsAppDbContext _dbContext;

    public ServiceProviderRepository(EsAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ServiceProvider>> GetActiveAsync()
    {
        return await _dbContext.ServiceProvider
            .AsNoTracking()
            .Include(item => item.Currency)
            .Where(item => item.StateRecord == "A" && item.Currency.StateRecord == "A")
            .Select(item => new ServiceProvider
            {
                serviceProviderId = item.ServiceProviderId,
                service = item.Service,
                currencyId = item.CurrencyId,
                currency = item.Currency.Currency
            })
            .ToListAsync();
    }

    public async Task<ServiceProvider?> GetByIdAsync(Guid serviceProviderId)
    {
        return await _dbContext.ServiceProvider
            .AsNoTracking()
            .Include(item => item.Currency)
            .Where(item => item.StateRecord == "A" &&
                item.ServiceProviderId == serviceProviderId &&
                item.Currency.StateRecord == "A")
            .Select(item => new ServiceProvider
            {
                serviceProviderId = item.ServiceProviderId,
                service = item.Service,
                currencyId = item.CurrencyId,
                currency = item.Currency.ShortName
            })
            .FirstOrDefaultAsync();
    }
}
