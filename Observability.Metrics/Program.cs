using Observability.Metrics.Models;
using Observability.Metrics.Repositories;
using Observability.Metrics.Services;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithMetrics(mpb => mpb
        //.AddAspNetCoreInstrumentation()
        //.AddRuntimeInstrumentation()
        //.AddProcessInstrumentation()
        .AddPrometheusExporter()
        //.AddMeter(PizzeriaMetrics.MeterName)
    );

builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<ICookProductService, CookProductService>();
builder.Services.AddSingleton<PizzeriaMetrics>();

var app = builder.Build();

app.MapPrometheusScrapingEndpoint();

app.MapGet("/sell/{productId}", async (int productId, IProductsRepository repository, ICookProductService cookProductService, PizzeriaMetrics pizzeriaMetrics) =>
{
    var product = repository.GetProduct(productId);
    if (product.Type is ProductType.Pizza)
    {
        await cookProductService.Cook(product);
    }
    pizzeriaMetrics.ProductSold(product);
    return Results.Ok();
});

app.Run();