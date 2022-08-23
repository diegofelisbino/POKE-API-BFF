namespace Pokedex.Api.Configurations
{
    public static class SentryConfig
    {
        public static void AddSentryConfig(IWebHostBuilder webHost)
        {

            webHost.UseSentry();

        }

        public static IApplicationBuilder UseSentryConfig(this IApplicationBuilder app)
        {
            app.UseSentryTracing();

            return app;
        }

    }

}
