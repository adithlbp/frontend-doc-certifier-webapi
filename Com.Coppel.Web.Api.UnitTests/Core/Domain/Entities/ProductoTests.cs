using Com.Coppel.Web.Api.Core.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Com.Coppel.Web.Api.UnitTests.Core.Domain.Entities
{
    public class ProductoTests
    {
        [Fact]
        public void Create_WithValidData_ShouldCreateProducto()
        {
            // Arrange
            var nombre = "Producto Test";
            var descripcion = "Descripción del producto test";
            var precio = 100.50m;

            // Act
            var producto = Producto.Create(nombre, descripcion, precio);

            // Assert
            producto.Should().NotBeNull();
            producto.Id.Should().NotBeEmpty();
            producto.Nombre.Should().Be(nombre);
            producto.Descripcion.Should().Be(descripcion);
            producto.PrecioUnitario.Should().Be(precio);
        }

        [Theory]
        [InlineData("", "Descripción válida", 100.0)]
        [InlineData("   ", "Descripción válida", 100.0)]
        [InlineData(null, "Descripción válida", 100.0)]
        public void Create_WithInvalidNombre_ShouldThrowArgumentException(string nombre, string descripcion, decimal precio)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Producto.Create(nombre, descripcion, precio));
            exception.Message.Should().Be("El nombre es requerido");
        }

        [Theory]
        [InlineData("Nombre válido", "", 100.0)]
        [InlineData("Nombre válido", "   ", 100.0)]
        [InlineData("Nombre válido", null, 100.0)]
        public void Create_WithInvalidDescripcion_ShouldThrowArgumentException(string nombre, string descripcion, decimal precio)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Producto.Create(nombre, descripcion, precio));
            exception.Message.Should().Be("La descripción es requerida");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100.50)]
        public void Create_WithInvalidPrecio_ShouldThrowArgumentException(decimal precio)
        {
            // Arrange
            var nombre = "Producto Test";
            var descripcion = "Descripción válida";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Producto.Create(nombre, descripcion, precio));
            exception.Message.Should().Be("El precio debe ser mayor a cero");
        }

        [Fact]
        public void UpdatePrice_WithValidPrice_ShouldUpdatePrice()
        {
            // Arrange
            var producto = Producto.Create("Test", "Test Description", 100m);
            var nuevoPrecio = 150.75m;

            // Act
            producto.UpdatePrice(nuevoPrecio);

            // Assert
            producto.PrecioUnitario.Should().Be(nuevoPrecio);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-50.25)]
        public void UpdatePrice_WithInvalidPrice_ShouldThrowArgumentException(decimal nuevoPrecio)
        {
            // Arrange
            var producto = Producto.Create("Test", "Test Description", 100m);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => producto.UpdatePrice(nuevoPrecio));
            exception.Message.Should().Be("El precio debe ser mayor a cero");
        }
    }
}