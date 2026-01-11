using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EsApp.Api.Extensions.Swagger;

public class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;
        operation.Deprecated |= apiDescription.IsDeprecated();

        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions.First(
                pd => pd.Name == parameter.Name);

            parameter.Description ??= description.ModelMetadata.Description;

            if (parameter.Schema.Default == null && description.DefaultValue != null)
                parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());

            parameter.Required |= description.IsRequired;
        }

        operation.Parameters.Add(new OpenApiParameter()
        {
            Name = "PageName",
            In = ParameterLocation.Header,
            Description = "Valor de la pagina actual para la consulta.",
            Required = true
        });
    }
}
