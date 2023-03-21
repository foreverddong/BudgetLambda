var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.AddJsonConsole();


var app = builder.Build();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
