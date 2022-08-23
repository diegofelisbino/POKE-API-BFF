using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Pokedex.Api.Configurations;
using Pokedex.Api.Extensions;



var builder = WebApplication.CreateBuilder(args);


LoggerConfig.AddLoggingConfig (builder.Host, builder.Environment);

ElmahIoConfig.AddElmahIoConfig(builder.Services);

SentryConfig.AddSentryConfig(builder.WebHost);

ApiConfig.AddApiConfig(builder.Services);

SwaggerConfig.AddSwaggerConfig(builder.Services);

DependecyInjectionConfig.ResolveDependencies(builder.Services);




var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

ElmahIoConfig.UseElmahIoConfig(app);

SentryConfig.UseSentryConfig(app);

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

SwaggerConfig.UseSwaggerConfig(app, apiVersionDescriptionProvider);

ApiConfig.UseApiConfig(app, app.Configuration);

app.MapControllers();

app.Run();

public partial class Program { }
