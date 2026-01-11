using EsApp.Api.Extensions;
using EsApp.Api.Extensions.Swagger;
using EsApp.CROSS.Shared;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using EsApp.Authentication.JWT;
using EsApp.CROSS.Logger;
using EsApp.CROSS.Encrypt;
using EsApp.Persistence;
using EsApp.Application;
using EsApp.Api.Middleware;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Asp.Versioning;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExtension();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddAntiforgery();
builder.Services.AddConfigureCors(builder.Configuration);
builder.Services.AddEndpoints(typeof(Program).Assembly);

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddAuthorization();
builder.Services.AddAuthenticationJwt();
builder.Services.AddLogger();
builder.Services.AddSecurity();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerExtension(provider);

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();

RouteGroupBuilder versionedGroup = app
    .MapGroup("api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

app.UseCors("PolicyEsApp");
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthentication(); // Asegúrate de que esto esté antes de UseAuthorization
app.UseAuthorization();
app.UseMiddleware<RequestBodyMiddleware>();
app.MapEndpoints(versionedGroup);

app.Run();
