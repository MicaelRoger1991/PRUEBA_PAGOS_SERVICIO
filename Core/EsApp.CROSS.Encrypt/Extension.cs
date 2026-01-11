using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EsApp.CROSS.Encrypt;

public static class Extension
{
    private static readonly string _sectionSecretKey = "secretKey";
    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        IConfiguration configuration;
        using (var serviceProvider = services.BuildServiceProvider())
        {
            configuration = serviceProvider.GetService<IConfiguration>();
        }
        string options = configuration.GetSection(_sectionSecretKey).Value;
        services.AddSingleton<ISecurityEncrypt, SecurityEncrypt>(sp =>
        {
            return new SecurityEncrypt(options);
        });
        return services;
    }
}
