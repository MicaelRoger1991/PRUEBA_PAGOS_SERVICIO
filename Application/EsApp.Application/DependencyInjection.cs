using System;
using EsApp.Application.Auth;
using EsApp.Application.Customers;
using EsApp.Application.Parametrics;
using EsApp.Application.ServiceProvider;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EsApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IServiceProviderService, ServiceProviderService>();
        services.AddScoped<ICustomersService, CustomersService>();


        return services;
    }
}
