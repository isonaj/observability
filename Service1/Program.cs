using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var ActivitySource = new ActivitySource("Service1");

app.MapGet("/", () => "Hello World!");

app.MapGet("/test/{value}", async (int value) =>  {
    using var activity = ActivitySource.StartActivity("test");
    activity?.SetTag("Value", value);
    await Task.Delay(value);
});

app.Run();
