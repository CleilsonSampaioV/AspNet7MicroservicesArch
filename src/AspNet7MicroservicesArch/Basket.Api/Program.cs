using Basket.Api.GrpcServices;
using Basket.Api.Repository;
using Discount.Grpc.Protos;
using MassTransit;
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

app.Run();

static void ConfigureServices(
    IServiceCollection services,
    IConfiguration configuration
)
{
    // Redis Configuration
    services.AddStackExchangeRedisCache(opt =>
    {
        opt.Configuration = configuration.GetValue<string>("CachingSettings:ConnectionString");
    });

    // General Configuration
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddScoped<IBasketRepository, BasketRepository>();
    services.AddAutoMapper(Assembly.GetExecutingAssembly());
    services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
    });

    // Grpc Configuration
    services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
    opt.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]));
    services.AddScoped<DiscountGrpcServices>();

    // MassTransit-RabbitMQ Configuration
    services.AddMassTransit(config =>
    {
        config.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host(configuration["EventBusSettings:HostAddress"]);
        });
    });
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
    app.UseCors();
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}