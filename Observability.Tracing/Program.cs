using Observability.Tracing.Services;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

var serviceName = "Observability.Tracing";

builder.Services.AddOpenTelemetry().WithTracing(tcb =>
{
    tcb
        .AddSource(serviceName)
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName: serviceName))
        //.AddConsoleExporter();
        //.SetSampler(new ParentBasedSampler(new TraceIdRatioBasedSampler(0.5)))
        //.AddAspNetCoreInstrumentation()
        .AddJaegerExporter();
});

builder.Services.AddSingleton(TracerProvider.Default.GetTracer(serviceName));
builder.Services.AddTransient<ISomeService, SomeService>();

var app = builder.Build();

//app.UseMiddleware<RequestHeadersLoggingMiddleware>();

app.MapGet("/", async (ISomeService someService) => await someService.SomeMethodWithTracing());

app.Run();