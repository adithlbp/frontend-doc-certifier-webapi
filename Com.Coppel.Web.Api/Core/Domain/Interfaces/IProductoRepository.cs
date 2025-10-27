using System.Linq.Expressions;
using Com.Coppel.Web.Api.Core.Domain.Entities;
using Com.Coppel.Web.Api.Core.Application.Common;

namespace Com.Coppel.Web.Api.Core.Domain.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de productos
    /// </summary>
    public interface IProductoRepository
    {
        /// <summary>
        /// Obtiene todos los productos
        /// </summary>
        Task<List<Producto>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        Task<Producto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca productos que cumplan con un predicado
        /// </summary>
        Task<List<Producto>> FindAsync(Expression<Func<Producto, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca un producto que cumpla con un predicado
        /// </summary>
        Task<Producto?> FindOneAsync(Expression<Func<Producto, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Realiza una consulta paginada
        /// </summary>
        Task<PaginatedResponse<Producto>> GetPaginatedAsync(PaginatedQueryParams queryParams, CancellationToken cancellationToken = default);

        /// <summary>
        /// Agrega un nuevo producto
        /// </summary>
        Task<Guid> AddAsync(Producto producto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        Task UpdateAsync(Producto producto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Elimina un producto
        /// </summary>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Guarda los cambios en la base de datos
        /// </summary>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}