using Elastic.Serilog.Sinks;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;

namespace Observability.Logging.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOurCustomLogging(this IServiceCollection services, ILoggingBuilder loggingBuilder)
    {
        AddDefaultAspNetLogging();
        //AddLoggingWithoutProviders(loggingBuilder);
        //AddLoggingWithManualAddedConsoleProvider(loggingBuilder);
        //AddLoggingWithAdvancedSerilog(loggingBuilder);
        
        return services;
    }

    private static void AddDefaultAspNetLogging()
    {
        return;
    }

    private static void AddLoggingWithoutProviders(ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.ClearProviders();
    }
    
    private static void AddLoggingWithManualAddedConsoleProvider(ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.ClearProviders();
        
        loggingBuilder.AddConsole();
        loggingBuilder.AddBasicSerilog();
    }
    
    private static void AddLoggingWithAdvancedSerilog(ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.ClearProviders();
        
        loggingBuilder.AddAdvancedSerilog();
    }

    private static ILoggingBuilder AddBasicSerilog(this ILoggingBuilder loggingBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
         loggingBuilder.AddSerilog();

         return loggingBuilder;
    }

    private static ILoggingBuilder AddAdvancedSerilog(this ILoggingBuilder loggingBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("environment", "development")
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .WriteTo.Async(config => config.Console(new ExceptionAsObjectJsonFormatter(inlineFields: true)))
            .WriteTo.Async(config => config.Elasticsearch())
            .CreateLogger();
        loggingBuilder.AddSerilog();

        return loggingBuilder;
    }
}