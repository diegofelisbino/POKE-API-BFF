using Serilog;
using Sentry;

namespace Pokedex.Api.Configurations
{
    public static class LoggerConfig
    {
        public static void AddLoggingConfig(IHostBuilder host, IWebHostEnvironment environment)
        {
            string appSettings = environment.EnvironmentName == "Production" ? "appsettings.json" : "appsettings.Development.json";

            var config = new ConfigurationBuilder()
                .AddJsonFile(appSettings)
                .Build();

            var serilog = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            host.UseSerilog(serilog);

        }

    }

}
