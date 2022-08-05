using Microsoft.Extensions.DependencyInjection;
using Pokedex.Api.AutoMapper;
using Pokedex.Api.Configurations;
using Pokedex.Application.AutoMapper;
using Pokedex.Application.Contracts;
using Pokedex.Application.Interfaces;
using Pokedex.Application.Services;
using Refit;

namespace Pokedex.Test
{
    public static class DependecyInjectionConfig
    {
        public static ServiceCollection ResolverDependencias()
        {
            var services =  new ServiceCollection();    

            services.AddScoped<IPokemonService, PokemonService>();

            services.AddRefitClient<IPokemonApi>().ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(ApiConfig.BASE_ADRESS_EXTERNAL_API);
            });

            services.AddAutoMapper(typeof(ApiConfigurationMapping));
            services.AddScoped<ApiConfigurationMapping>();

            services.AddAutoMapper(typeof(ApplicationConfigurationMapping));
            services.AddScoped<ApplicationConfigurationMapping>();

            return services;

        }

    }

}
