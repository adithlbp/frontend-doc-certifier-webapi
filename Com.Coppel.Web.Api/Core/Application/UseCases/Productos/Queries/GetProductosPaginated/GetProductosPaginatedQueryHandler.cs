using Com.Coppel.Web.Api.Core.Application.Common;
using Com.Coppel.Web.Api.Core.Application.Common.Logging;
using Com.Coppel.Web.Api.Core.Application.DTOs;
using Com.Coppel.Web.Api.Core.Domain.Interfaces;

namespace Com.Coppel.Web.Api.Core.Application.UseCases.Productos.Queries.GetProductosPaginated
{
    /// <summary>
    /// Manejador del query para obtener productos paginados
    /// </summary>
    public class GetProductosPaginatedQueryHandler
    {
        private readonly IProductoRepository _repository;
        private readonly ILogger<GetProductosPaginatedQueryHandler> _logger;

        public GetProductosPaginatedQueryHandler(IProductoRepository repository, ILogger<GetProductosPaginatedQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PaginatedResponse<ProductoDto>> Handle(GetProductosPaginatedQuery query, CancellationToken cancellationToken)
        {
            ProductoLogMessages.BuscandoProductosPaginados(_logger, query.QueryParams.Page, query.QueryParams.Limit, null);

            var paginatedProductos = await _repository.GetPaginatedAsync(query.QueryParams, cancellationToken);

            var productosDto = paginatedProductos.Data.Select(p => new ProductoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                PrecioUnitario = p.PrecioUnitario
            }).ToList();

            ProductoLogMessages.ProductosObtenidos(_logger, productosDto.Count, paginatedProductos.Total, null);

            return new PaginatedResponse<ProductoDto>(
                paginatedProductos.Page,
                paginatedProductos.Limit,
                paginatedProductos.Total,
                productosDto
            );
        }
    }
}