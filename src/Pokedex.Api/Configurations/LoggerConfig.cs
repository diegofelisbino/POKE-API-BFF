using Serilog;



namespace Pokedex.Api.Configurations
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IHostBuilder host,  IConfiguration configuration)
        {
            //var config = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json").Build();

            //var seriLog = new LoggerConfiguration()
            //    .ReadFrom.Configuration(config)
            //    .CreateLogger();

            var serilog = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.Console(
                                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
                                outputTemplate: "{Timestamp: HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
                                )
                .WriteTo.File(
                                "C:\\GitHubLabs\\Logs\\POKE-API-BFF\\log_.txt",
                                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                                outputTemplate: "{Timestamp: HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}",
                                rollingInterval: RollingInterval.Day
                              )                  
                .CreateLogger();                                


            host.UseSerilog(serilog); // assume o serilog como provider padrão            

            services.AddElmahIo(o =>
            {
                o.ApiKey = "a655fb4a6e2c4547980e8c376af30ea4";
                o.LogId = new Guid("e624981c-efd5-43d0-a807-db8e21732c6b");
            });


            return services;

        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }

    }
}
