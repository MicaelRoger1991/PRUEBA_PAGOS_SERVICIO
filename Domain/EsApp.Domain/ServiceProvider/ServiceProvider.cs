using System;

namespace EsApp.Domain.ServiceProvider;

public class ServiceProvider
{
    public Guid serviceProviderId { get; set; }
    public string service { get; set; } = string.Empty;
    public Guid currencyId { get; set; }
    public string currency { get; set; } = string.Empty;
}
