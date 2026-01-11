using EsApp.Application.Parametrics.Response;
using EsApp.CROSS.Shared.Abstractions;

namespace EsApp.Application.Parametrics;

public interface ICurrencyService
{
    Task<Result<IReadOnlyList<CurrencyResponse>>> GetCurrenciesAsync(CancellationToken cancellationToken);
}
