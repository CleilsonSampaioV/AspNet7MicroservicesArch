using Microsoft.OpenApi.Models;
using Shopping.Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(
    builder.Services,
    builder.Configuration
);

var app = builder.Build();

Configure(
    app,
    app.Environment,
    builder.Configuration
);

app.Run();

static void ConfigureServices(
    IServiceCollection services,
    IConfiguration configuration
)
{
    services.AddHttpClient<ICatalogService, CatalogService>(c =>
                c.BaseAddress = new Uri(configuration["ApiSettings:CatalogUrl"]));

    services.AddHttpClient<IBasketService, BasketService>(c =>
        c.BaseAddress = new Uri(configuration["ApiSettings:BasketUrl"]));

    services.AddHttpClient<IOrderService, OrderService>(c =>
        c.BaseAddress = new Uri(configuration["ApiSettings:OrderingUrl"]));

    services.AddControllers();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shopping.Aggregator", Version = "v1" });
    });
}

static void Configure(
    IApplicationBuilder app,
    IHostEnvironment env,
    IConfiguration configuration
)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping.Aggregator v1"));
    }

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}