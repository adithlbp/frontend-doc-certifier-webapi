using Com.Coppel.Web.Api.Core.Application.Common;
using Com.Coppel.Web.Api.Core.Domain.Entities;
using Com.Coppel.Web.Api.Infrastructure.Persistence.Context;
using Com.Coppel.Web.Api.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Com.Coppel.Web.Api.IntegrationTests.Infrastructure.Repositories
{
    public class ProductoRepositoryIntegrationTests : IDisposable
    {
        private readonly CoppelDbContext _context;
        private readonly ProductoRepository _repository;

        public ProductoRepositoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<CoppelDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new CoppelDbContext(options);
            _repository = new ProductoRepository(_context);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        [Fact]
        public async Task AddAsync_WithValidProducto_ShouldSaveToDatabase()
        {
            // Arrange
            var producto = Producto.Create("Producto Test", "Descripción test", 100.50m);

            // Act
            var id = await _repository.AddAsync(producto);
            await _repository.SaveChangesAsync();

            // Assert
            id.Should().Be(producto.Id);
            
            var savedProducto = await _repository.GetByIdAsync(id);
            savedProducto.Should().NotBeNull();
            savedProducto!.Nombre.Should().Be("Producto Test");
            savedProducto.Descripcion.Should().Be("Descripción test");
            savedProducto.PrecioUnitario.Should().Be(100.50m);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingId_ShouldReturnProducto()
        {
            // Arrange
            var producto = Producto.Create("Producto Test", "Descripción test", 75.25m);
            await _repository.AddAsync(producto);
            await _repository.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(producto.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(producto.Id);
            result.Nombre.Should().Be("Producto Test");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var result = await _repository.GetByIdAsync(nonExistingId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPaginatedAsync_WithValidParams_ShouldReturnPaginatedResults()
        {
            // Arrange - Crear una nueva instancia para este test
            var options = new DbContextOptionsBuilder<CoppelDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new CoppelDbContext(options);
            var repository = new ProductoRepository(context);

            // Crear exactamente 3 productos para este test
            for (int i = 1; i <= 3; i++)
            {
                var producto = Producto.Create($"TestPaginado{i}", $"Descripción {i}", i * 10m);
                await repository.AddAsync(producto);
            }
            await repository.SaveChangesAsync();

            var queryParams = new PaginatedQueryParams
            {
                Page = 1,
                Limit = 2
            };

            // Act
            var result = await repository.GetPaginatedAsync(queryParams);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.Total.Should().Be(2);
            result.Page.Should().Be(1);
            result.Limit.Should().Be(2);
        }

        [Fact]
        public async Task UpdateAsync_WithValidProducto_ShouldUpdateInDatabase()
        {
            // Arrange
            var producto = Producto.Create("Producto Original", "Descripción original", 50m);
            await _repository.AddAsync(producto);
            await _repository.SaveChangesAsync();

            // Modificar el producto
            producto.UpdatePrice(75m);

            // Act
            await _repository.UpdateAsync(producto);
            await _repository.SaveChangesAsync();

            // Assert
            var updatedProducto = await _repository.GetByIdAsync(producto.Id);
            updatedProducto.Should().NotBeNull();
            updatedProducto!.PrecioUnitario.Should().Be(75m);
        }

        [Fact]
        public async Task DeleteAsync_WithExistingId_ShouldRemoveFromDatabase()
        {
            // Arrange
            var producto = Producto.Create("Producto a eliminar", "Descripción", 25m);
            await _repository.AddAsync(producto);
            await _repository.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(producto.Id);
            await _repository.SaveChangesAsync();

            // Assert
            var deletedProducto = await _repository.GetByIdAsync(producto.Id);
            deletedProducto.Should().BeNull();
        }

        [Fact]
        public async Task FindAsync_WithPredicate_ShouldReturnMatchingProducts()
        {
            // Arrange
            var producto1 = Producto.Create("Laptop Gaming", "Laptop para juegos", 1500m);
            var producto2 = Producto.Create("Mouse Gaming", "Mouse para juegos", 50m);
            var producto3 = Producto.Create("Teclado Oficina", "Teclado para oficina", 30m);

            await _repository.AddAsync(producto1);
            await _repository.AddAsync(producto2);
            await _repository.AddAsync(producto3);
            await _repository.SaveChangesAsync();

            // Act
            var gamingProducts = await _repository.FindAsync(p => p.Nombre.Contains("Gaming"));

            // Assert
            gamingProducts.Should().HaveCount(2);
            gamingProducts.Should().Contain(p => p.Nombre == "Laptop Gaming");
            gamingProducts.Should().Contain(p => p.Nombre == "Mouse Gaming");
        }
    }
}