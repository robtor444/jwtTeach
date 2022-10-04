using JwtTeach;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var startup = new Startup(builder.Configuration);


startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.

var serviceLogger = (ILogger<Startup>)app.Services.GetService(typeof(ILogger<Startup>));

startup.Configure(app, app.Environment, serviceLogger);

app.Run();

