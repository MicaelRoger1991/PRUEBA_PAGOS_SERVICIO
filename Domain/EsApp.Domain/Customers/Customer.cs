using System;

namespace EsApp.Domain.Customers;

public class Customer
{
    public Guid customerId { get; set; }
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; } = string.Empty;
    public string documentNumber { get; set; } = string.Empty;
}
