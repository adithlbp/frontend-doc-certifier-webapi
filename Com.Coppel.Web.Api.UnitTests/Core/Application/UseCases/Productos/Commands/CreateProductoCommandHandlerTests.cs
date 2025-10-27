using Com.Coppel.Web.Api.Core.Application.UseCases.Productos.Commands.CreateProducto;
using Com.Coppel.Web.Api.Core.Domain.Entities;
using Com.Coppel.Web.Api.Core.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Com.Coppel.Web.Api.UnitTests.Core.Application.UseCases.Productos.Commands
{
    public class CreateProductoCommandHandlerTests
    {
        private readonly Mock<IProductoRepository> _repositoryMock;
        private readonly Mock<ILogger<CreateProductoCommandHandler>> _loggerMock;
        private readonly CreateProductoCommandHandler _handler;

        public CreateProductoCommandHandlerTests()
        {
            _repositoryMock = new Mock<IProductoRepository>();
            _loggerMock = new Mock<ILogger<CreateProductoCommandHandler>>();
            _handler = new CreateProductoCommandHandler(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCreateProductoAndReturnId()
        {
            // Arrange
            var command = new CreateProductoCommand
            {
                Nombre = "Producto Test",
                Descripcion = "Descripción del producto test",
                PrecioUnitario = 100.50m
            };

            var expectedId = Guid.NewGuid();
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Producto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(expectedId);
            _repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(expectedId);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Producto>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData("", "Descripción válida", 100.0)]
        [InlineData("   ", "Descripción válida", 100.0)]
        [InlineData(null, "Descripción válida", 100.0)]
        public async Task Handle_WithInvalidNombre_ShouldThrowArgumentException(string nombre, string descripcion, decimal precio)
        {
            // Arrange
            var command = new CreateProductoCommand
            {
                Nombre = nombre,
                Descripcion = descripcion,
                PrecioUnitario = precio
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            exception.Message.Should().Be("El nombre es requerido");
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Producto>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData("Nombre válido", "", 100.0)]
        [InlineData("Nombre válido", "   ", 100.0)]
        [InlineData("Nombre válido", null, 100.0)]
        public async Task Handle_WithInvalidDescripcion_ShouldThrowArgumentException(string nombre, string descripcion, decimal precio)
        {
            // Arrange
            var command = new CreateProductoCommand
            {
                Nombre = nombre,
                Descripcion = descripcion,
                PrecioUnitario = precio
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            exception.Message.Should().Be("La descripción es requerida");
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Producto>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100.50)]
        public async Task Handle_WithInvalidPrecio_ShouldThrowArgumentException(decimal precio)
        {
            // Arrange
            var command = new CreateProductoCommand
            {
                Nombre = "Nombre válido",
                Descripcion = "Descripción válida",
                PrecioUnitario = precio
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            exception.Message.Should().Be("El precio debe ser mayor a cero");
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Producto>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            var command = new CreateProductoCommand
            {
                Nombre = "Producto Test",
                Descripcion = "Descripción del producto test",
                PrecioUnitario = 100.50m
            };

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Producto>(), It.IsAny<CancellationToken>()))
                          .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _handler.Handle(command, CancellationToken.None));
            
            exception.Message.Should().Be("Database error");
        }
    }
}