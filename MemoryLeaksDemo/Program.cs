using System.Text;
using JetBrains.Profiler.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/inefficient", () =>
{
    // Start collecting profiling data
    MeasureProfiler.StartCollectingData();

    // Simulate some work with a performance bottleneck
    string result = SimulateWorkInefficiently();

    // Stop collecting data and save the snapshot
    MeasureProfiler.StopCollectingData();

    MeasureProfiler.SaveData();
    
    // MeasureProfiler.Detach();

    return Results.Text(result);
});

app.MapGet("/efficient", () =>
{
    // Start collecting profiling data
    MeasureProfiler.StartCollectingData();

    // Simulate some work with a performance bottleneck
    string result = SimulateWorkEfficiently();

    // Stop collecting data and save the snapshot
    MeasureProfiler.StopCollectingData();

    MeasureProfiler.SaveData();

    // MeasureProfiler.Detach();

    return Results.Text(result);
});

app.Run();

// Inefficient method with a performance bottleneck
static string SimulateWorkInefficiently()
{
    string result = "";
    for (int i = 0; i < 10000; i++)
    {
        // Inefficient string concatenation in a loop
        result += i.ToString() + " ";
    }

    return result.Trim();
}

// Optimized method
static string SimulateWorkEfficiently()
{
    // Use StringBuilder for efficient string manipulation
    StringBuilder sb = new StringBuilder();
    for (int i = 0; i < 10000; i++)
    {
        sb.Append(i.ToString()).Append(" ");
    }

    return sb.ToString().Trim();
}