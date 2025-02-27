using System.Reflection;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticConfiguration:Uri"]!))
    {
        AutoRegisterTemplate = true, // Ensure index template is registered
        IndexFormat = $"applogs-{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".","-")}-" +
        $"{builder.Environment.EnvironmentName?.ToLower().Replace(".", "-")}-" +
        $"logs-{DateTime.UtcNow:yyyy-MM}", // Custom index format
        NumberOfShards = 2,
        NumberOfReplicas =1 
    })
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Use Serilog as the logging provider
builder.Host.UseSerilog();

try
{
    Log.Information("Application is starting...");

    // Add services to the container
    builder.Services.AddRazorPages();

    builder.Services.AddRefitClient<ICatalogService>()
        .ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
        });

    builder.Services.AddRefitClient<IBasketService>()
        .ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
        });

    builder.Services.AddRefitClient<IOrderingService>()
        .ConfigureHttpClient(c =>
        {
            c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
        });

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
    app.MapRazorPages();

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
