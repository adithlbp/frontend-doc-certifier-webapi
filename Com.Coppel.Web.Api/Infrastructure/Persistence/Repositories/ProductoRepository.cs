using System.Linq.Expressions;
using Com.Coppel.Web.Api.Core.Application.Common;
using Com.Coppel.Web.Api.Core.Domain.Entities;
using Com.Coppel.Web.Api.Core.Domain.Interfaces;
using Com.Coppel.Web.Api.Infrastructure.Persistence.Context;
using Com.Coppel.Web.Api.Infrastructure.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Com.Coppel.Web.Api.Infrastructure.Persistence.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly CoppelDbContext _context;
        private readonly DbSet<Producto> _productos;

        public ProductoRepository(CoppelDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _productos = context.Set<Producto>();
        }

        public async Task<List<Producto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _productos.ToListAsync(cancellationToken);
        }

        public async Task<Producto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _productos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<Producto>> FindAsync(Expression<Func<Producto, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _productos.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<Producto?> FindOneAsync(Expression<Func<Producto, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _productos.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<PaginatedResponse<Producto>> GetPaginatedAsync(PaginatedQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = _productos.ApplyQueryParams(queryParams);
            var total = await query.CountAsync(cancellationToken);
            var data = await query.Skip(queryParams.Offset).Take(queryParams.Limit).ToListAsync(cancellationToken);

            return new PaginatedResponse<Producto>(
                page: queryParams.Page,
                limit: queryParams.Limit,
                total: total,
                data: data
            );
        }

        public async Task<Guid> AddAsync(Producto producto, CancellationToken cancellationToken = default)
        {
            await _productos.AddAsync(producto, cancellationToken);
            return producto.Id;
        }

        public Task UpdateAsync(Producto producto, CancellationToken cancellationToken = default)
        {
            _context.Entry(producto).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var producto = await _productos.FindAsync([id], cancellationToken);
            if (producto != null)
            {
                _productos.Remove(producto);
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}