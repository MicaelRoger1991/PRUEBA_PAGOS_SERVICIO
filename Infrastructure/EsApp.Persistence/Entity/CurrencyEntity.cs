using System;

namespace EsApp.Persistence.Entity;

public class CurrencyEntity
{
    public Guid CurrencyId { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string StateRecord { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime? ModificationDate { get; set; }
}
