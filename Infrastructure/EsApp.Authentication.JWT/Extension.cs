using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EsApp.Authentication.JWT;

public static class Extension
{
    private static string keyJwt;
    private static readonly string _sectionKeyJwt = "secretKeyJWT";
    public static IServiceCollection AddAuthenticationJwt(this IServiceCollection services)
    {
        IConfiguration configuration;
        using (var serviceProvider = services.BuildServiceProvider())
        {
            configuration = serviceProvider.GetService<IConfiguration>()!;
            keyJwt = configuration.GetSection(_sectionKeyJwt).Value!;
        }
        services.AddSingleton<IManagerAuthentication, ManagerAuthentication>(sp =>
        {
            return new ManagerAuthentication(configuration);
        });


        var keyBytes = Encoding.ASCII.GetBytes(keyJwt);

        services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(config =>
        {
            config.RequireHttpsMetadata = false;
            config.SaveToken = true;
            config.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}
