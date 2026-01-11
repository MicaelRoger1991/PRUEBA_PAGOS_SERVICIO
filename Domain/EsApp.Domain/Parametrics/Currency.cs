using System;

namespace EsApp.Domain.Parametrics;

public class Currency
{
    public Guid currencyId { get; set; }
    public string currency { get; set; } = string.Empty;
    public string shortName { get; set; } = string.Empty;
}
