var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы для API и Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Генерирует документ OpenAPI

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

// Пример эндпоинта
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