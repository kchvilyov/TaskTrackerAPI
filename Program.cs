using Microsoft.EntityFrameworkCore;
using TaskTrackerAPI.Data;
using TaskTrackerAPI.Services;

// C# (Program.cs) - аналог @Configuration в Spring
var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллеры и API Explorer
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Генерирует документ OpenAPI

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db")); // Простая БД для демо

// Регистрируем сервис. Это аналог @Service в Spring
builder.Services.AddScoped<TaskService>();

var app = builder.Build();

// Включаем HTTPS
app.UseHttpsRedirection();

// Настройка Swagger в режиме разработки
if (app.Environment.IsDevelopment())
{
    // Отдаёт JSON по адресу: /swagger/v1/swagger.json
    app.UseSwagger();

    // Подключает UI по адресу: /swagger
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskTrackerAPI v1");
        options.RoutePrefix = "swagger"; // Доступ по http://localhost:5297/swagger
    });
}

// Важно: подключаем маршрутизацию для контроллеров
app.MapControllers();

// Пример эндпоинта (можно оставить для теста)
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

// Запись для ответа
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}