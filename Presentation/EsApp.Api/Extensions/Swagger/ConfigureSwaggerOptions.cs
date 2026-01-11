using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EsApp.Api.Extensions.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public IConfiguration Configuration { get; }

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
    {
        _provider = provider;
        Configuration = configuration;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, Configuration));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, IConfiguration config)
    {
        var info = new OpenApiInfo
        {
            Title = config.GetValue<string>("SwaggerSettings:Title"),
            Version = description.ApiVersion.ToString(),
            Description = config.GetValue<string>("SwaggerSettings:Description"),
            Contact = new OpenApiContact
            {
                Name = config.GetValue<string>("SwaggerSettings:Name"),
                Email = config.GetValue<string>("SwaggerSettings:Email"),
                Url = new Uri(config.GetValue<string>("SwaggerSettings:Url")!)
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " <strong>Esta version de Api ha sido deprecada.</strong>";
        }

        return info;
    }
}
