var builder = WebApplication.CreateBuilder(args);

// add services  to the container

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Configure the HTTP request pipeline

app.Run();
