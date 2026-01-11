using System;

namespace EsApp.Api.Middleware;

public class RequestBodyMiddleware
{
    private readonly RequestDelegate _next;
    public RequestBodyMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();

        if (this._next != null)
        {
            await this._next.Invoke(context);
        }
    }
}
