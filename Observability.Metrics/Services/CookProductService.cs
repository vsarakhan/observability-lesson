using System.Diagnostics;
using Observability.Metrics.Models;

namespace Observability.Metrics.Services;

public interface ICookProductService
{
    Task Cook(Product product);
}

public class CookProductService(
    PizzeriaMetrics metrics
    ) : ICookProductService
{
    public async Task Cook(Product product)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        await Task.Delay(new Random().Next(1, 10) * 1000);
        stopwatch.Stop();
        metrics.RecordProductCooking(product, stopwatch.ElapsedMilliseconds);
    }
}