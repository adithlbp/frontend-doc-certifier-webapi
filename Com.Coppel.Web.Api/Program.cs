using Com.Coppel.Web.Api.Infrastructure.Configuration;
using Com.Coppel.Web.Api.Infrastructure.Persistence.Context;
using Com.Coppel.Web.Api.Core.Domain.Interfaces;
using Com.Coppel.Web.Api.Infrastructure.Clients;
using Com.Coppel.Web.Api.Infrastructure.Persistence.Repositories;
using Com.Coppel.Web.Api.Core.Application.UseCases.Evaluations.Commands.CreateEvaluation;
using Com.Coppel.Web.Api.Core.Application.UseCases.Evaluations.Queries.GetEvaluation;
using Com.Coppel.Web.Api.Core.Application.UseCases.Revisions.Queries.GetRevisions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;

namespace Com.Coppel.Web.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers: Agrega los controladores a la aplicacion
            builder.Services.AddControllers();

            // Authentication: Removido para simplificar el template

            // Swagger: Agrega la documentacion de Swagger a la aplicacion
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new() { Title = "Frontend Document Certifier API", Version = "v1" });
            });

            // Database Configuration - PostgreSQL con Cloud SQL
            var connectionString = builder.Configuration.GetConnectionString("SpecDb");
            builder.Services.AddDbContext<SpecDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            // Health Checks: Agrega los health checks a la aplicacion
            builder.Services
                .AddHealthChecks()
                .AddDbContextCheck<SpecDbContext>();

            // CORS Configuration - Configurable por env
            var allowedOrigins = builder.Configuration["CORS_ALLOWED_ORIGINS"] ?? "*";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyCorsPolicyName", policy =>
                {
                    if (allowedOrigins == "*")
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    }
                    else
                    {
                        var origins = allowedOrigins.Split(',');
                        policy
                            .WithOrigins(origins)
                            .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                            .AllowAnyHeader();
                    }
                });
            });

            // Registro de repositorios
            builder.Services.AddScoped<IEvaluationRepository, EvaluationRepository>();
            builder.Services.AddScoped<IRevisionRepository, RevisionRepository>();
            builder.Services.AddScoped<ICriterionRepository, CriterionRepository>();

            // Registro de Genius API Client
            builder.Services.AddHttpClient<IGeniusApiClient, GeniusApiClient>(client =>
            {
                var baseUrl = builder.Configuration["GENIUS_BASE_URL"];
                if (!string.IsNullOrEmpty(baseUrl))
                {
                    client.BaseAddress = new Uri(baseUrl);
                }
            });

            // Registro de Handlers
            builder.Services.AddScoped<CreateEvaluationCommandHandler>();
            builder.Services.AddScoped<GetEvaluationQueryHandler>();
            builder.Services.AddScoped<GetRevisionsQueryHandler>();

            // Clean Architecture Layers (mantener para compatibilidad)
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();
            builder.Services.AddPresentation();

            // Logging Configuration
            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

            var app = builder.Build();

            // Si el ambiente es desarrollo
            if (app.Environment.IsDevelopment())
            {
                // Agrega el middleware de Swagger
                app.UseSwagger();
                // Agrega el middleware de Swagger UI, para visualizar la documentacion de Swagger
                app.UseSwaggerUI();

                // Muestra una pagina de error detallada
                app.UseDeveloperExceptionPage();

            }
            else
            {
                // uso de HSTS (HTTP Strict Transport Security)
                app.UseHsts();
                // Usa el middleware de redireccionamiento HTTPS, ejemplo: http://localhost:5000 -> https://localhost:5001
                app.UseHttpsRedirection();
            }

            // Health Checks: Configurar antes de los middlewares personalizados
            app.UseHealthChecks("/health");

            // Custom Middlewares
            app.UseCustomMiddlewares();

            // Habilita la politica CORS con el nombre "MyCorsPolicyName" de forma global
            app.UseCors("MyCorsPolicyName");

            // Habilita el enrutamiento
            app.UseRouting();

            // Authentication removido

            app.MapControllers();

            app.Run();
        }
    }
}