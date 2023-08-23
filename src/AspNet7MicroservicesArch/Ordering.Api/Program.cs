using EventBus.Messages.Common;
using MassTransit;
using Microsoft.OpenApi.Models;
using Ordering.Api.EventBusConsumer;
using Ordering.Api.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
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

app.MigrateDatabase<OrderContext>((context, services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed
        .SeedAsync(context, logger)
        .Wait();
});

app.Run();

static void ConfigureServices(
    IServiceCollection services,
    IConfiguration configuration
)
{
    // General Configuration
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddAutoMapper(Assembly.GetExecutingAssembly());
    services.AddApplicationServices();
    services.AddInfrastructureServices(configuration);
    services.AddScoped<BasketCheckoutConsumer>();
    services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
    });

    // MassTransit-RabbitMQ Configuration
    services.AddMassTransit(config =>
    {

        config.AddConsumer<BasketCheckoutConsumer>();

        config.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host(configuration["EventBusSettings:HostAddress"]);

            cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
            {
                c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
            });
        });
    });

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
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
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.Api v1"));
    }
    app.UseCors();
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
