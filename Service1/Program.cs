using System.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var serviceName = "Service1";
var serviceVersion = "1.0.0";

var builder = WebApplication.CreateBuilder(args);

// Configure important OpenTelemetry settings, the console exporter, and instrumentation library
builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
{
    tracerProviderBuilder
        .AddConsoleExporter()
        .AddSource(serviceName)
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation();
});

var app = builder.Build();

var ActivitySource = new ActivitySource("Service1");

app.MapGet("/", () => "Hello World!");

app.MapGet("/test/{value}", async (int value) =>  {
    using var activity = ActivitySource.StartActivity("test");
    activity?.SetTag("Value", value);
    await Task.Delay(value);
});

app.Run();
