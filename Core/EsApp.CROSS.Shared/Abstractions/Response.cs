using System;

namespace EsApp.CROSS.Shared.Abstractions;

public class Response
{
    public object? Data { get; set; }
    public Refresh? TokenValue { get; set; }
}

public class Response<T>
{
    public T? Data { get; set; }
    public Refresh? TokenValue { get; set; }
}

public class Refresh
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}