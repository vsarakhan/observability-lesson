using System.Diagnostics.Metrics;
using Observability.Metrics.Models;

namespace Observability.Metrics.Services;

public class PizzeriaMetrics
{
    public static readonly string MeterName = "Observability.Metrics.Pizzeria";

    private const string ProductSoldMetricName = "pizzeria.product.sold";
    private const string ProductCookingTimeMetricName = "pizzeria.product.cooking.time";
    
    private readonly Counter<int> _productSoldCounter;
    
    public PizzeriaMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        _productSoldCounter = meter.CreateCounter<int>(ProductSoldMetricName);
    }

    public void ProductSold(Product product)
    {
        _productSoldCounter.Add(1, new KeyValuePair<string, object?>[]
        {
            new("product.name", product.Name),
            new("product.type", product.Type.ToString()),
        });
    }
    
    public void RecordProductCooking(Product product, double cookingTime)
    {
        
    }
}