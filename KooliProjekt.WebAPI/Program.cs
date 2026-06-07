using FluentValidation;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace KooliProjekt.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddScoped<IAutoRepository, AutoRepository>();
            builder.Services.AddScoped<ITootajaRepository, TootajaRepository>();
            builder.Services.AddScoped<IOperatsioonRepository, OperatsioonRepository>();
            builder.Services.AddScoped<IOperatsiooniTyypRepository, OperatsiooniTyypRepository>();

            var applicationAssembly = typeof(ErrorHandlingBehavior<,>).Assembly;

            builder.Services.AddValidatorsFromAssembly(applicationAssembly);

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(applicationAssembly);
                config.AddOpenBehavior(typeof(ErrorHandlingBehavior<,>));
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(TransactionalBehavior<,>));
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("BlazorCorsPolicy", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseCors("BlazorCorsPolicy");

            app.MapControllers();

            // ВАЖНО:
            // В интеграционных тестах используется InMemoryDatabase.
            // SeedData.Generate вызывает db.Database.Migrate(),
            // а Migrate не работает с InMemory.
            // Поэтому в Testing среде SeedData не запускаем.
            if (!app.Environment.IsEnvironment("Testing"))
            {
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    SeedData.Generate(db);
                }
            }

            app.Run();
        }
    }
}