using Com.Coppel.Web.Api.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Com.Coppel.Web.Api.IntegrationTests.Common
{
    public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remover el DbContext existente
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<CoppelDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Agregar DbContext en memoria para tests
                services.AddDbContext<CoppelDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                // Configurar logging para tests
                services.AddLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Warning);
                });

                // Crear la base de datos
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<CoppelDbContext>();
                
                db.Database.EnsureCreated();
            });

            builder.UseEnvironment("Testing");
        }
    }
}