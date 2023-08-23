using Catalog.Api.Data;
using Catalog.Api.Repository;

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
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddScoped<ICatalogContext, CatalogContext>();
    services.AddScoped<IProductRepository, ProductRepository>();
}

static void Configure(
    IApplicationBuilder app,
    IHostEnvironment env,
    IConfiguration configuration
)
{
    // Configure the HTTP request pipeline.
    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
    }
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}