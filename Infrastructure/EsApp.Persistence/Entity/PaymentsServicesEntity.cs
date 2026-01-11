using System;

namespace EsApp.Persistence.Entity;

public class PaymentsServicesEntity
{
    public Guid PaymentsId { get; set; }
    public Guid UsersMasterId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ServiceProviderId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StateRecord { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime? ModificationDate { get; set; }

    public UsersMasterEntity UsersMaster { get; set; } = null!;
    public CustomersEntity Customer { get; set; } = null!;
    public ServiceProviderEntity ServiceProvider { get; set; } = null!;
}
