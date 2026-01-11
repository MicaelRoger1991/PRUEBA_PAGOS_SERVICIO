using System;
using EsApp.Domain.Parametrics;
using EsApp.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EsApp.Persistence.Repository;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly EsAppDbContext _dbContext;

    public CurrencyRepository(EsAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Currency>> GetActiveAsync()
    {
        return await _dbContext.Currency
            .AsNoTracking()
            .Where(item => item.StateRecord == "A")
            .Select(item => new Currency
            {
                currencyId = item.CurrencyId,
                currency = item.Currency,
                shortName = item.ShortName
            })
            .ToListAsync();
    }
}
