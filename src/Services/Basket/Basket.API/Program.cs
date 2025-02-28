using BuildingBlocks.Messaging.MassTransit;
using Common.Logging;
using Discount.Grpc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog using Common.Logging
SeriLogger.ConfigureLogging(builder.Configuration, builder.Environment.EnvironmentName);

// Use Serilog as the logging provider
builder.Host.UseSerilog();

// Enable HTTP/2 unencrypted support (for development purposes)
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

// Add services to the container

// Application Services
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
try
{
    Log.Information("Application is starting...");
    // Data Services
    builder.Services.AddMarten(opts =>
    {
        opts.Connection(builder.Configuration.GetConnectionString("Database")!);
        opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    }).UseLightweightSessions();

    builder.Services.AddScoped<IBasketRepository, BasketRepository>();
    builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
    builder.Services.AddStackExchangeRedisCache(option =>
    {
        option.Configuration = builder.Configuration.GetConnectionString("Redis");
    });

    // gRPC Services with custom HttpClientHandler for certificate validation fix ssl
    //var handler = new HttpClientHandler
    //{
    //    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
    //};

    //builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(option =>
    //{
    //    option.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
    //}).ConfigurePrimaryHttpMessageHandler(() => handler);

    //Grpc Services

    //Teacher fix
    builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
    {
        options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        return handler;
    }).AddHttpMessageHandler<LoggingDelegatingHandler>();

    // Async Communication Services
    builder.Services.AddMessageBroker(builder.Configuration);

    // Cross-Cutting Services
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();

    builder.Services.AddHealthChecks()
        .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
        .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

    // Build the app
    var app = builder.Build();

    // Configure the HTTP request pipeline
    app.MapCarter();

    app.UseExceptionHandler(opts =>
    {
        // Custom exception handling logic
    });

    app.UseHealthChecks("/health",
        new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start.");
}
finally
{
    Log.CloseAndFlush(); // Ensure logs are properly flushed on shutdown
}
