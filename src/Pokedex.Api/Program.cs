using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Pokedex.Api.Configurations;
using System.Web.Http;
using System.Web.Http.Dispatcher;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddEndpointsApiExplorer();


ApiConfig.AddApiConfig(builder.Services);

SwaggerConfig.AddSwaggerConfig(builder.Services);

DependecyInjectionConfig.ResolveDependencies(builder.Services);



var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

SwaggerConfig.UseSwaggerConfig(app, apiVersionDescriptionProvider);

ApiConfig.UseApiConfig(app, app.Configuration);

app.MapControllers();

app.Run();

public partial class Program { }
