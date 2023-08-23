using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(
    builder.Services,
    builder.Configuration
);

var app = builder.Build();

Configure(
    app,
    app.Environment,
    builder.Configuration,
    builder.Host
);

app.Run();

static void ConfigureServices(
    IServiceCollection services,
    IConfiguration configuration
)
{
    services.AddOcelot()
        .AddCacheManager(
        settings => settings.WithDictionaryHandle()
        );
}

static async void Configure(
    IApplicationBuilder app,
    IHostEnvironment env,
    IConfiguration configuration,
    IHostBuilder hostBuilder
)
{
    hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
    });

    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Hello World!");
        });
    });
    await app.UseOcelot();
}