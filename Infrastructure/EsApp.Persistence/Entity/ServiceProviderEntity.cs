using System;

namespace EsApp.Persistence.Entity;

public class ServiceProviderEntity
{
    public Guid ServiceProviderId { get; set; }
    public string Service { get; set; } = string.Empty;
    public Guid CurrencyId { get; set; }
    public string StateRecord { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime? ModificationDate { get; set; }

    public CurrencyEntity Currency { get; set; } = null!;
    public ICollection<PaymentsServicesEntity> Payments { get; set; } = new List<PaymentsServicesEntity>();
}
