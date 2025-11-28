using System.Text;
using DocumentFlowAPI.Base;
using DocumentFlowAPI.Configuration;
using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Middleware;
using DocumentFlowAPI.Repositories.Template;
using DocumentFlowAPI.Repositories.User;
using DocumentFlowAPI.Services.Auth;
using DocumentFlowAPI.Services.Template;
using DocumentFlowAPI.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IJwtService, JwtService>();


        //Настройка JWT
        services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

        //Настройка аутентификации
        var jwtSettings = Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        var key = Encoding.ASCII.GetBytes(jwtSettings!.SecretKey);
        //Настройка авторизации
        services.AddAuthentication(options =>
        {
            // Устанавливаем JWT как схему аутентификации по умолчанию
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;  // Для разработки (в продакшене должно быть true)
            options.SaveToken = true;             // Сохраняем токен в контексте
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,                    // Проверяем подпись
                IssuerSigningKey = new SymmetricSecurityKey(key),   // Ключ для проверки
                ValidateIssuer = true,                             // Проверяем издателя
                ValidIssuer = jwtSettings.Issuer,                 // Владидный издатель
                ValidateAudience = true,                          // Проверяем аудиторию
                ValidAudience = jwtSettings.Audience,             // Валидная аудитория
                ValidateLifetime = true,                          // Проверяем время жизни
                ClockSkew = TimeSpan.Zero                         // Не даем дополнительного времени
            };
        });

        //Добавляем авторизацию
        services.AddAuthorization();

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

            // Добавляем поддержку JWT авторизации в Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
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

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
