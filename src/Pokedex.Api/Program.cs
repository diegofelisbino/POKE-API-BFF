using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Api.Configurations;
using Pokedex.Api.Extensions;


var builder = WebApplication.CreateBuilder(args);

LoggerConfig.AddLoggingConfig (builder.Host, builder.Environment);

ElmahIoConfig.AddElmahIoConfig(builder.Services);

SentryConfig.AddSentryConfig(builder.WebHost);

HealthCheckConfig.AddHealthConfig(builder.Services);

ApiConfig.AddApiConfig(builder.Services);

SwaggerConfig.AddSwaggerConfig(builder.Services);

DependecyInjectionConfig.ResolveDependencies(builder.Services);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

ElmahIoConfig.UseElmahIoConfig(app);

SentryConfig.UseSentryConfig(app);

SwaggerConfig.UseSwaggerConfig(app, apiVersionDescriptionProvider);

ApiConfig.UseApiConfig(app, app.Configuration);

HealthCheckConfig.UseHealthConfig(app);

app.MapControllers();

app.Run();

public partial class Program { }
