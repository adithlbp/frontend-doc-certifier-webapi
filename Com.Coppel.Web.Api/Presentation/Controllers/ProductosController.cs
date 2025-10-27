using Com.Coppel.Web.Api.Core.Application.Common;
using Com.Coppel.Web.Api.Core.Application.Common.Logging;
using Com.Coppel.Web.Api.Core.Application.DTOs;
using Com.Coppel.Web.Api.Core.Application.UseCases.Productos.Commands.CreateProducto;
using Com.Coppel.Web.Api.Core.Application.UseCases.Productos.Queries.GetProducto;
using Com.Coppel.Web.Api.Core.Application.UseCases.Productos.Queries.GetProductosPaginated;
using Com.Coppel.Web.Api.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Com.Coppel.Web.Api.Presentation.Controllers
{
    /// <summary>
    /// Controlador para la gestión de productos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly CreateProductoCommandHandler _createHandler;
        private readonly GetProductoQueryHandler _getHandler;
        private readonly GetProductosPaginatedQueryHandler _getPaginatedHandler;
        private readonly TraceIdentifier _traceIdentifier;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(
            CreateProductoCommandHandler createHandler,
            GetProductoQueryHandler getHandler,
            GetProductosPaginatedQueryHandler getPaginatedHandler,
            TraceIdentifier traceIdentifier,
            ILogger<ProductosController> logger)
        {
            _createHandler = createHandler;
            _getHandler = getHandler;
            _getPaginatedHandler = getPaginatedHandler;
            _traceIdentifier = traceIdentifier;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<SuccessApiResponse<ProductoDto>>> GetById(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            ProductoLogMessages.ObteniendoProductoPorId(_logger, id, null);
            
            var query = new GetProductoQuery(id);
            var producto = await _getHandler.Handle(query, cancellationToken);
            
            return Ok(ApiResponse.Ok(producto, _traceIdentifier.Id));
        }

        /// <summary>
        /// Obtiene productos con paginación
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<SuccessApiResponse<PaginatedResponse<ProductoDto>>>> GetPaginated(
            [FromQuery] PaginatedQueryParams queryParams,
            CancellationToken cancellationToken)
        {
            ProductoLogMessages.ObteniendoProductosPaginados(_logger, null);
            
            var query = new GetProductosPaginatedQuery(queryParams);
            var productos = await _getPaginatedHandler.Handle(query, cancellationToken);
            
            return Ok(ApiResponse.Ok(productos, _traceIdentifier.Id));
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<SuccessApiResponse<Guid>>> Create(
            [FromBody] CreateProductoDto createDto,
            CancellationToken cancellationToken)
        {
            ProductoLogMessages.CreandoNuevoProducto(_logger, createDto.Nombre, null);
            
            var command = new CreateProductoCommand
            {
                Nombre = createDto.Nombre,
                Descripcion = createDto.Descripcion,
                PrecioUnitario = createDto.PrecioUnitario
            };
            
            var id = await _createHandler.Handle(command, cancellationToken);
            
            return CreatedAtAction(nameof(GetById), new { id }, ApiResponse.Ok(id, _traceIdentifier.Id));
        }
    }
}