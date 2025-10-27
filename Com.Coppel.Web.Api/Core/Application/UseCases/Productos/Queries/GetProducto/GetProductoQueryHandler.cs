using Com.Coppel.Web.Api.Core.Application.Common.Logging;
using Com.Coppel.Web.Api.Core.Application.DTOs;
using Com.Coppel.Web.Api.Core.Domain.Interfaces;
using Com.Coppel.Web.Api.Core.Domain.Exceptions;

namespace Com.Coppel.Web.Api.Core.Application.UseCases.Productos.Queries.GetProducto
{
    /// <summary>
    /// Manejador del query para obtener producto
    /// </summary>
    public class GetProductoQueryHandler
    {
        private readonly IProductoRepository _repository;
        private readonly ILogger<GetProductoQueryHandler> _logger;

        public GetProductoQueryHandler(IProductoRepository repository, ILogger<GetProductoQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ProductoDto> Handle(GetProductoQuery query, CancellationToken cancellationToken)
        {
            ProductoLogMessages.BuscandoProducto(_logger, query.Id, null);

            if (query.Id == Guid.Empty)
                throw new ArgumentException("El ID es requerido");

            var producto = await _repository.GetByIdAsync(query.Id, cancellationToken);

            if (producto == null)
            {
                ProductoLogMessages.ProductoNoEncontrado(_logger, query.Id, null);
                throw new RecordNotFoundException<Guid>(query.Id);
            }

            ProductoLogMessages.ProductoEncontrado(_logger, producto.Nombre, null);

            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                PrecioUnitario = producto.PrecioUnitario
            };
        }
    }
}