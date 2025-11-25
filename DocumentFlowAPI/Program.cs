using DocumentFlowAPI.Base;
using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Repositories.Template;
using DocumentFlowAPI.Repositories.User;
using DocumentFlowAPI.Services.Template;
using DocumentFlowAPI.Services.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы
builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ITemplateService, TemplateService>();
builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


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

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies()
                    .UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                        new MySqlServerVersion(new Version(8, 0, 21)));
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
