using DocumentFlowAPI.Base;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы
builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// Настройка Swagger
builder.Services.AddEndpointsApiExplorer(); // важно: добавляет описание эндпоинтов для Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Document Flow API",
        Version = "v1"
    });
});

var app = builder.Build();

// Настройка middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Document Flow API v1");
        c.RoutePrefix = string.Empty; // Swagger доступен по http://localhost:xxxx/
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); // ✅ для API (вместо MapControllerRoute)

app.Run();
