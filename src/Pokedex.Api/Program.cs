using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Pokedex.Api.Configurations;
using Pokedex.Api.Extensions;


var builder = WebApplication.CreateBuilder(args);

LoggerConfig.AddLoggingConfiguration (builder.Services, builder.Host, builder.Configuration);

ApiConfig.AddApiConfig(builder.Services);

SwaggerConfig.AddSwaggerConfig(builder.Services);

DependecyInjectionConfig.ResolveDependencies(builder.Services);



var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

LoggerConfig.UseLoggingConfiguration(app);

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

SwaggerConfig.UseSwaggerConfig(app, apiVersionDescriptionProvider);

ApiConfig.UseApiConfig(app, app.Configuration);

app.MapControllers();

app.Run();

public partial class Program { }
