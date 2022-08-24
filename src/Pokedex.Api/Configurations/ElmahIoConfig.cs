namespace Pokedex.Api.Configurations
{
    public static class ElmahIoConfig
    {
        public static IServiceCollection AddElmahIoConfig(this IServiceCollection services)
        {   
            services.AddElmahIo(o =>
            {
                o.ApiKey = "a655fb4a6e2c4547980e8c376af30ea4";
                o.LogId = new Guid("e624981c-efd5-43d0-a807-db8e21732c6b");
            });            

            return services;

        }

        public static IApplicationBuilder UseElmahIoConfig(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }

    }

}
