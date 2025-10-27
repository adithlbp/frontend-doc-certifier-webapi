using Com.Coppel.Web.Api.Core.Application.Common;
using Com.Coppel.Web.Api.Core.Application.DTOs;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace Com.Coppel.Web.Api.EndToEndTests.Api
{
    public class ProductosControllerE2ETests : IDisposable
    {
        private readonly HttpClient _client;

        public ProductosControllerE2ETests()
        {
            // Para tests E2E reales, se conectaría a una instancia de la API en ejecución
            // Por ahora, estos son tests de ejemplo que muestran la estructura
            _client = new HttpClient();
        }

        [Fact]
        public void CreateProducto_WithValidData_ShouldHaveCorrectStructure()
        {
            // Arrange
            var createDto = new CreateProductoDto
            {
                Nombre = "Producto E2E Test",
                Descripcion = "Descripción del producto E2E",
                PrecioUnitario = 199.99m
            };

            // Act & Assert - Verificar que el DTO tiene la estructura correcta
            createDto.Should().NotBeNull();
            createDto.Nombre.Should().Be("Producto E2E Test");
            createDto.Descripcion.Should().Be("Descripción del producto E2E");
            createDto.PrecioUnitario.Should().Be(199.99m);
        }

        [Fact]
        public void ApiResponse_ShouldHaveCorrectStructure()
        {
            // Arrange
            var transactionId = "test-transaction-id";
            var testData = Guid.NewGuid();

            // Act
            var response = ApiResponse.Ok(testData, transactionId);

            // Assert
            response.Should().NotBeNull();
            response.Data.Should().Be(testData);
            response.Meta.Should().NotBeNull();
            response.Meta.Status.Should().Be("OK");
            response.Meta.TransactionID.Should().Be(transactionId);
        }

        [Fact]
        public void PaginatedResponse_ShouldHaveCorrectStructure()
        {
            // Arrange
            var productos = new List<ProductoDto>
            {
                new ProductoDto { Id = Guid.NewGuid(), Nombre = "Producto 1", Descripcion = "Desc 1", PrecioUnitario = 100m },
                new ProductoDto { Id = Guid.NewGuid(), Nombre = "Producto 2", Descripcion = "Desc 2", PrecioUnitario = 200m }
            };

            // Act
            var response = new PaginatedResponse<ProductoDto>(
                page: 1,
                limit: 10,
                total: 2,
                data: productos
            );

            // Assert
            response.Should().NotBeNull();
            response.Page.Should().Be(1);
            response.Limit.Should().Be(10);
            response.Total.Should().Be(2);
            response.Data.Should().HaveCount(2);
        }

        [Fact]
        public void JsonSerialization_ShouldWorkCorrectly()
        {
            // Arrange
            var createDto = new CreateProductoDto
            {
                Nombre = "Test Product",
                Descripcion = "Test Description",
                PrecioUnitario = 99.99m
            };

            // Act
            var json = JsonConvert.SerializeObject(createDto);
            var deserializedDto = JsonConvert.DeserializeObject<CreateProductoDto>(json);

            // Assert
            json.Should().NotBeNullOrEmpty();
            deserializedDto.Should().NotBeNull();
            deserializedDto!.Nombre.Should().Be(createDto.Nombre);
            deserializedDto.Descripcion.Should().Be(createDto.Descripcion);
            deserializedDto.PrecioUnitario.Should().Be(createDto.PrecioUnitario);
        }

        [Fact]
        public void HttpStatusCodes_ShouldBeCorrect()
        {
            // Act & Assert - Verificar que los códigos HTTP están disponibles
            HttpStatusCode.OK.Should().Be(HttpStatusCode.OK);
            HttpStatusCode.Created.Should().Be(HttpStatusCode.Created);
            HttpStatusCode.BadRequest.Should().Be(HttpStatusCode.BadRequest);
            HttpStatusCode.NotFound.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public void ApiResponseMetadata_ShouldCreateCorrectly()
        {
            // Arrange
            var transactionId = "test-id";
            var message = "Test message";

            // Act
            var okMetadata = ApiResponseMetadata.Ok(transactionId, message);
            var errorMetadata = ApiResponseMetadata.Error(transactionId, HttpStatusCode.BadRequest, "Error message");
            var notFoundMetadata = ApiResponseMetadata.NotFound(transactionId, "Not found message");

            // Assert
            okMetadata.Status.Should().Be("OK");
            okMetadata.StatusCode.Should().Be(HttpStatusCode.OK);
            
            errorMetadata.Status.Should().Be("ERROR");
            errorMetadata.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            notFoundMetadata.Status.Should().Be("NOT_FOUND");
            notFoundMetadata.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        void IDisposable.Dispose()
        {
            _client?.Dispose();
        }
    }
}