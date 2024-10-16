using OpenTelemetry.Trace;

namespace Observability.Tracing.Services;

public interface ISomeService
{
    Task SomeMethod();
    
    Task SomeMethodWithTracing();
    
    Task SomeMethodWithAttributes();
    
    Task SomeMethodWithUnsuccessfulOperation();

    Task SomeMethodWithFireAndForgetOperation();
}

public class SomeService(
    Tracer tracer,
    ILogger<SomeService> logger
    )
    : ISomeService
{
    public async Task SomeMethod()
    {
        var result = await  DoA();
        if (result > 5)
        {
            await DoB();
        }
        else
        {
            await DoC();
        }
    }
    
    public async Task SomeMethodWithTracing()
    {
        using var span = tracer.StartActiveSpan("Some method with tracing");
        var result = await DoAWithTracing();
        if (result > 5)
        {
            await DoBWithTracing();
        }
        else
        {
            await DoCWithTracing();
        }
    }
    
    public async Task SomeMethodWithAttributes()
    {
        using var span = tracer.StartActiveSpan("Some method with tracing");
        span.SetAttribute("userId", new Random().Next());
        span.SetAttribute("userLogin", Guid.NewGuid().ToString());
        
        var result = await DoAWithTracing();
        if (result > 5)
        {
            await DoBWithTracing();
        }
        else
        {
            await DoCWithTracing();
        }
    }
    
    public async Task SomeMethodWithUnsuccessfulOperation()
    {
        using var span = tracer.StartActiveSpan("Some method with tracing");
        span.SetAttribute("userId", new Random().Next());
        span.SetAttribute("userLogin", Guid.NewGuid().ToString());

        try
        {
            var result = await DoAWithTracing();
            if (result > 5)
            {
                await DoBWithTracing();
            }
            else
            {
                throw new ApplicationException("Oh no");
            }
        }
        catch (Exception ex)
        {
            span.SetStatus(Status.Error.WithDescription(ex.Message));
            span.RecordException(ex);
        }
    }
    
    public async Task SomeMethodWithFireAndForgetOperation()
    {
        SpanContext? spanContext = null;
        using (var span = tracer.StartActiveSpan("Some method with tracing"))
        {
            span.SetAttribute("userId", new Random().Next());
            span.SetAttribute("userLogin", Guid.NewGuid().ToString());
            logger.LogInformation("Hello world!");
            await DoAWithTracing();
            await DoBWithTracing();
            spanContext = span.Context;
        }
        FireAndForget(spanContext.Value);
    }


    private async Task<int> DoA()
    {
        await Task.Delay(new Random().Next(0, 1000));
        return new Random().Next(0, 11);
    }
    
    private async Task<int> DoB()
    {
        await Task.Delay(new Random().Next(3000, 10000));
        return new Random().Next(7, 11);
    }
    
    private async Task<int> DoC()
    {
        await Task.Delay(0);
        return new Random().Next(0, 2);
    }
    
    private async Task<int> DoAWithTracing()
    {
        using var span = tracer.StartActiveSpan("Do A with tracing");
        await Task.Delay(new Random().Next(0, 2000));
        return new Random().Next(0, 11);
    }
    
    private async Task<int> DoBWithTracing()
    {
        using var span = tracer.StartActiveSpan("Do B with tracing");
        await Task.Delay(new Random().Next(3000, 10000));
        return new Random().Next(7, 11);
    }
    
    private async Task<int> DoCWithTracing()
    {
        using var span = tracer.StartActiveSpan("Do C with tracing");
        await Task.Delay(0);
        return new Random().Next(0, 2);
    }

    private async void FireAndForget(SpanContext context)
    {
        using var span = tracer.StartActiveSpan("Some fire and forget operation", links: [new Link(context)]);
        await Task.Delay(new Random().Next(4000, 8000));
        span.AddEvent("Fire and forget operation completed");
    }
}