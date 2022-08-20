


using Microsoft.Extensions.Options;
using Pokedex.Api.AutoMapper;
using Pokedex.Application.AutoMapper;
using Pokedex.Application.Contracts;
using Pokedex.Application.Interfaces;
using Pokedex.Application.Notificacoes;
using Pokedex.Application.Services;
using Refit;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Pokedex.Api.Configurations
{
    public static class DependecyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<IPokemonService, PokemonService>();

            services.AddRefitClient<IPokemonApi>().ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("https://pokeapi.co/api/v2");
            });
         
            services.AddAutoMapper(typeof(ApiConfigurationMapping));
            services.AddScoped<ApiConfigurationMapping>();

            services.AddAutoMapper(typeof(ApplicationConfigurationMapping));
            services.AddScoped<ApplicationConfigurationMapping>();

            services.AddScoped<INotificador, Notificador>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }
    }
    
}
