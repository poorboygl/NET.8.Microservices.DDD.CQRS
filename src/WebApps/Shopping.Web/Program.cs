using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog using Common.Logging
SeriLogger.ConfigureLogging(builder.Configuration, builder.Environment.EnvironmentName);

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
