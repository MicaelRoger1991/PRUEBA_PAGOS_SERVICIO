using System;

namespace EsApp.Domain.Payments;

public class Payment
{
    public Guid paymentId { get; set; }
    public Guid customerId { get; set; }
    public Guid serviceProviderId { get; set; }
    public Guid usersMasterId { get; set; }
    public decimal amount { get; set; }
    public string status { get; set; } = string.Empty;
    public DateTime createdAt { get; set; }
    public string serviceProvider { get; set; } = string.Empty;
}
