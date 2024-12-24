using BuildingBlock.Behaviors;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
//* Add service  to the container

builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddCarter();

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();



var app = builder.Build();

//* Configure the HTTP request pipeline
app.MapCarter();

app.Run();
