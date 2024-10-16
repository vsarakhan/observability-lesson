﻿using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var serviceName = "Observability.Tracing.Client";

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource(serviceName)
    //.AddHttpClientInstrumentation()
    .SetResourceBuilder(
        ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName))
    .AddJaegerExporter()
    .Build();

using var httpClient = new HttpClient();

var response = await httpClient.GetAsync("http://localhost:5072");
response.EnsureSuccessStatusCode();
Console.WriteLine("Response received successfully.");