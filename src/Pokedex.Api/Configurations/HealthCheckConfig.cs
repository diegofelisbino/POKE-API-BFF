using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Pokedex.Api.Configurations
{
    public static class HealthCheckConfig
    {
        public static IServiceCollection AddHealthConfig(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddUrlGroup(new Uri("https://pokeapi.co"), name: "pokeapi-base")
                .AddUrlGroup(new Uri("https://pokeapi.co/api/v2/pokemon/1"), name: "pokeapi-id")
                .AddUrlGroup(new Uri("https://pokeapi.co/api/v2/pokemon/?offset=0&limit=1"), name: "pokeapi-paginado");

            services.AddHealthChecksUI()
                .AddInMemoryStorage();

            return services;

        }

        public static IApplicationBuilder UseHealthConfig(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/api/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/api/hc-ui";

                options.ResourcesPath = "/api/hc-ui-resources";

                options.UseRelativeApiPath = false;
                options.UseRelativeResourcesPath = false;
                options.UseRelativeWebhookPath = false;                
            });

            return app;
        }
    }

}
