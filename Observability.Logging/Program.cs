using Observability.Logging.Extensions;
using Observability.Logging.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOurCustomLogging(builder.Logging);

builder.Services.AddTransient<ISomeService, SomeService>();

var app = builder.Build();

app.MapGet("/", (ISomeService someService) => someService.SomeMethod());

app.Run();