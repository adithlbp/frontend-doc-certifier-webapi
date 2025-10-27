using Com.Coppel.Web.Api.Core.Application.UseCases.Productos.Commands.CreateProducto;
using Com.Coppel.Web.Api.Core.Application.UseCases.Productos.Queries.GetProducto;
using Com.Coppel.Web.Api.Core.Application.UseCases.Productos.Queries.GetProductosPaginated;
using Com.Coppel.Web.Api.Core.Domain.Interfaces;
using Com.Coppel.Web.Api.Infrastructure.Persistence.Context;
using Com.Coppel.Web.Api.Infrastructure.Persistence.Repositories;
using Com.Coppel.Web.Api.Presentation.Common;
using Com.Coppel.Web.Api.Presentation.Middleware;

using Microsoft.EntityFrameworkCore;

namespace Com.Coppel.Web.Api.Infrastructure.Configuration
{
    /// <summary>
    /// Configuración de inyección de dependencias
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Configura los servicios de infraestructura
        /// </summary>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext - Usando base de datos en memoria para desarrollo
            services.AddDbContext<CoppelDbContext>(options =>
                options.UseInMemoryDatabase("CoppelInMemoryDb"));

            // Repositorios
            services.AddScoped<IProductoRepository, ProductoRepository>();

            return services;
        }

        /// <summary>
        /// Configura los servicios de aplicación
        /// </summary>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Handlers de comandos
            services.AddScoped<CreateProductoCommandHandler>();

            // Handlers de queries
            services.AddScoped<GetProductoQueryHandler>();
            services.AddScoped<GetProductosPaginatedQueryHandler>();

            return services;
        }

        /// <summary>
        /// Configura los servicios de presentación
        /// </summary>
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            // Servicios comunes
            services.AddScoped<TraceIdentifier>();
            services.AddScoped<DomainExceptionMiddleware>();

            return services;
        }

        /// <summary>
        /// Configura los middlewares de la aplicación
        /// </summary>
        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
        {
            // Middleware para el seguimiento de identificadores de dominio
            app.UseMiddleware<DomainTraceIdentifierMiddleware>();

            // Middleware para el manejo de excepciones de dominio
            app.UseMiddleware<DomainExceptionMiddleware>();

            return app;
        }
    }
}