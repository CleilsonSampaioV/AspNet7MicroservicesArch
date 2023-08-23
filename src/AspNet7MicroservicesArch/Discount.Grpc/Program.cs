using Discount.Grpc.Extensions;
using Discount.Grpc.Repository;
using Discount.Grpc.Services;
using System.Reflection;

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

//app.MigrateDatabase<Program>();

app.Run();

static void Configure(
    IApplicationBuilder app,
    IHostEnvironment env,
    IConfiguration configuration
)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGrpcService<DiscountService>();

        endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    });
}

static void ConfigureServices(
    IServiceCollection services,
    IConfiguration configuration
)
{
    services.AddAutoMapper(Assembly.GetExecutingAssembly());
    services.AddScoped<IDiscountRepository, DiscountRepository>();
    services.AddGrpc();
}

