using DocumentFlowAPI;
using DocumentFlowAPI.Services.User;
using Scalar.AspNetCore;

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

app.MapSwagger("/openapi/{documentName}.json");
app.MapScalarApiReference();

app.MapControllers();

UserIdentity.Initialize(app.Services);

app.Run();
