using System;
using EsApp.Application.Parametrics.Response;
using EsApp.CROSS.Shared.Abstractions;
using EsApp.Domain.Parametrics;

namespace EsApp.Application.Parametrics;

public class CurrencyService : ICurrencyService
{
    private readonly ICurrencyRepository _currencyRepository;

    public CurrencyService(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<Result<IReadOnlyList<CurrencyResponse>>> GetCurrenciesAsync(CancellationToken cancellationToken)
    {
        var currencies = await _currencyRepository.GetActiveAsync();
        var result = currencies
            .Select(item => new CurrencyResponse(
                item.currencyId,
                item.currency,
                item.shortName
            ))
            .ToList();

        return Result.Success<IReadOnlyList<CurrencyResponse>>(result);
    }
}
