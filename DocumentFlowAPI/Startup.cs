using DocumentFlowAPI.Base;
using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Middleware;
using DocumentFlowAPI.Repositories.Template;
using DocumentFlowAPI.Repositories.User;
using DocumentFlowAPI.Services.Template;
using DocumentFlowAPI.Services.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace DocumentFlowAPI;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();


        //Настройка JWT

        //Настройка аутентификации

        //Настройка авторизации

        //Регистрация EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies()
                    .UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                        new MySqlServerVersion(new Version(8, 0, 21)),
                        options => options.EnableRetryOnFailure());
            });

        //Регистрация AutoMapper
        services.AddAutoMapper(typeof(Program));

        //Регистрация Swagger
        services.AddEndpointsApiExplorer(); // важно: добавляет описание эндпоинтов для Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Document Flow API",
                Version = "v1"
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Настройка pipeline для разработки
        app.UseErrorHandling();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Use(async (context, next) =>
        {
            Console.WriteLine("Before ErrorHandlingMiddleware");
            await next();
            Console.WriteLine("After ErrorHandlingMiddleware");
        });

        app.UseHttpsRedirection();
        app.UseAuthorization();
    }
}
