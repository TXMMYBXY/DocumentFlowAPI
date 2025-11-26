using DocumentFlowAPI;

var builder = WebApplication.CreateBuilder(args);

// Добавляем политику CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Создаем экземпляр Startup
var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Включаем CORS (должно быть ДО UseRouting и UseEndpoints)
app.UseCors("AllowAll");

// Конфигурируем pipeline
startup.Configure(app, app.Environment);

app.MapControllers(); // ✅ для API (вместо MapControllerRoute)

app.Run();
