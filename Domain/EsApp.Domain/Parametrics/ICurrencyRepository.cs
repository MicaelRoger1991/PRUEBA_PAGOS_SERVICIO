using System;

namespace EsApp.Domain.Parametrics;

public interface ICurrencyRepository
{
    Task<IReadOnlyList<Currency>> GetActiveAsync();
}
