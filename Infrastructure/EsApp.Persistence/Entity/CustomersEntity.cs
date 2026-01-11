using System;

namespace EsApp.Persistence.Entity;

public class CustomersEntity
{
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string StateRecord { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime? ModificationDate { get; set; }

    public ICollection<PaymentsServicesEntity> Payments { get; set; } = new List<PaymentsServicesEntity>();
}
