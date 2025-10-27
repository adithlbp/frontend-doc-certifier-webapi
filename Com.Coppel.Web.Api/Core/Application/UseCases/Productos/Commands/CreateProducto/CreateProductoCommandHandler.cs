using Com.Coppel.Web.Api.Core.Application.Common.Logging;
using Com.Coppel.Web.Api.Core.Domain.Entities;
using Com.Coppel.Web.Api.Core.Domain.Interfaces;

namespace Com.Coppel.Web.Api.Core.Application.UseCases.Productos.Commands.CreateProducto
{
    /// <summary>
    /// Manejador del comando para crear producto
    /// </summary>
    public class CreateProductoCommandHandler
    {
        private readonly IProductoRepository _repository;
        private readonly ILogger<CreateProductoCommandHandler> _logger;

        public CreateProductoCommandHandler(IProductoRepository repository, ILogger<CreateProductoCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateProductoCommand command, CancellationToken cancellationToken)
        {
            ProductoLogMessages.ProcesandoCreacionProducto(_logger, command.Nombre, null);

            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(command.Nombre))
                throw new ArgumentException("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(command.Descripcion))
                throw new ArgumentException("La descripción es requerida");

            if (command.PrecioUnitario <= 0)
                throw new ArgumentException("El precio debe ser mayor a cero");

            // Crear entidad de dominio
            var producto = Producto.Create(command.Nombre, command.Descripcion, command.PrecioUnitario);

            // Guardar en repositorio
            var id = await _repository.AddAsync(producto, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            ProductoLogMessages.ProductoCreadoExitosamente(_logger, id, null);
            return id;
        }
    }
}