using Com.Coppel.Web.Api.Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Com.Coppel.Web.Api.Infrastructure.Persistence.Context
{
    public class CoppelDbContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; }

        public CoppelDbContext(DbContextOptions<CoppelDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ProductoModelBuilder(modelBuilder.Entity<Producto>());
        }

        private static void ProductoModelBuilder(EntityTypeBuilder<Producto> productoBuilder)
        {
            productoBuilder.ToTable("Productos");
            productoBuilder.HasKey(p => p.Id);

            productoBuilder.Property(p => p.Nombre).HasMaxLength(128);
            productoBuilder.Property(p => p.Descripcion).HasMaxLength(512);
            productoBuilder.Property(p => p.PrecioUnitario).HasColumnType("decimal(18,2)");

            var initialData = new Producto[1000];
            var nonce = 0;

            var nombres = new[] { "Laptop", "Mouse", "Teclado", "Monitor", "Audifonos", "Bocinas", "Impresora", "Disco duro", "Memoria RAM", "Procesador" };
            var descripciones = new[] { "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla euismod, nisl quis aliquam ultricies, nunc nisl aliquet nunc, quis aliquam nisl nisl eu nisl. Sed euismod, nisl quis aliquam ultricies, nunc nisl aliquet nunc, quis aliquam nisl nisl eu nisl.", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla euismod, nisl quis aliquam ultricies, nunc nisl aliquet nunc, quis aliquam nisl nisl eu nisl. Sed euismod, nisl quis aliquam ultricies, nunc nisl aliquet nunc, quis aliquam nisl nisl eu nisl.", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla euismod, nisl quis aliquam ultricies, nunc nisl aliquet nunc, quis aliquam nisl nisl eu nisl. Sed euismod, nisl quis aliquam ultricies, nunc nisl aliquet nunc, quis aliquam nisl nisl eu nisl." };
            var precios = new[] { 10, 20, 50, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 2000, 5000, 10000 };

            for (int i = 0; i < initialData.Length; i++)
            {
                initialData[i] = new Producto
                {
                    Id = Guid.NewGuid(),
                    Nombre = nombres[nonce++ % nombres.Length],
                    Descripcion = descripciones[nonce++ % descripciones.Length],
                    PrecioUnitario = precios[nonce++ % precios.Length]
                };
            }

            productoBuilder.HasData(initialData);
        }
    }
}