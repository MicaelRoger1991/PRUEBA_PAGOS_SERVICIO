using System;
using EsApp.Application.Auth;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EsApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);
        services.AddScoped<IAuthService, AuthService>();


        return services;
    }
}
