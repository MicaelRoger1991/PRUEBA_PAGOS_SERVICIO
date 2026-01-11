using System;
using System.Reflection;
using EsApp.Api.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EsApp.Api.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                type.IsAssignableTo(typeof(IEndPoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndPoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static void AddConfigureCors(this IServiceCollection services, IConfiguration configuration) =>
        services.AddCors(options =>
        {
            var origenesPermitidos = configuration["ApplicationSettings:AllowedHosts"]!.Split(';');
            options.AddPolicy("PolicyEsApp", builder =>
                builder.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });
}
