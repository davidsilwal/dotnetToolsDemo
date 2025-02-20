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
var list = new List<string>();

app.MapGet("/leak", () =>
{
    MemoryProfiler.CollectAllocations(true);
    MemoryProfiler.GetSnapshot("Before Loops");

    for (var i = 0; i < 1000; i++)
    {
        list.Add(new string('a', 1000));
    }

    MemoryProfiler.GetSnapshot("After Loops");
    MemoryProfiler.CollectAllocations(false);

    MemoryProfiler.Detach();

    return "Leaked!";
});

app.Run();