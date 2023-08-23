using Discount.Api.Extensions;
using Discount.Api.Repository;

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

app.MigrateDatabase<Program>();

app.Run();

static void ConfigureServices(
    IServiceCollection services,
    IConfiguration configuration
)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddScoped<IDiscountRepository, DiscountRepository>();
}

static void Configure(
    IApplicationBuilder app,
    IHostEnvironment env,
    IConfiguration configuration
)
{
    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Discount.Api v1"));
    }
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
