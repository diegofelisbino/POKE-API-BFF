


using Pokedex.Api.AutoMapper;
using Pokedex.Application.AutoMapper;
using Pokedex.Application.Contracts;
using Pokedex.Application.Interfaces;
using Pokedex.Application.Services;
using Refit;

namespace Pokedex.Api.Configurations
{
    public static class DependecyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
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
