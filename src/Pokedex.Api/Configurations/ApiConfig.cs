using Microsoft.AspNetCore.Mvc;

namespace Pokedex.Api.Configurations
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services)
        {

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddControllers();
                        
            return services;

        }
        public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {                  
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            return app;
        }


    }
}