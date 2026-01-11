using System;

namespace EsApp.CROSS.Shared.Abstractions;

public class ServiceRequest
{
    public object? Data { get; set; }
    public string PageName { get; set; } = string.Empty;
}

public class ServiceRequest<T>
{
    public T? Data { get; set; }
    public string PageName { get; set; } = string.Empty;
}
