using System;
using EsApp.CROSS.Encrypt;
using EsApp.Domain.Auth;
using EsApp.Domain.Token;
using EsApp.Persistence.Context;
using EsApp.Persistence.DataAccess;
using EsApp.Persistence.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EsApp.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        List<ConnectionString> connectionStrings = configuration.GetSection("DataBaseSettings:ConnectionStrings").Get<List<ConnectionString>>()!;
        services.Configure<DataBaseSettings>(options =>
        {
            options.ConnectionStrings = connectionStrings;
        });
        services.AddKeyedScoped<IDataAccess, EsAppDb>("EsAppDb");

        services.AddDbContext<EsAppDbContext>((serviceProvider, options) =>
        {
            var encrypt = serviceProvider.GetRequiredService<ISecurityEncrypt>();
            var settings = serviceProvider.GetRequiredService<IOptions<DataBaseSettings>>();
            var connection = settings.Value.ConnectionStrings.First(x => x.Name.Equals("EsAppDb"));
            string password = encrypt.dencrypt(connection.Password);
            string connString = $"Host={connection.Server};Database={connection.DataBase};Username={connection.User};Password={password};Timeout={connection.Timeout}";
            options.UseNpgsql(connString);
        });

        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        return services;
    }
}
