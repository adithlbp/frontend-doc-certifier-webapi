using Com.Coppel.Web.Api.Core.Domain.Entities;
using Com.Coppel.Web.Api.Core.Domain.Interfaces;
using Com.Coppel.Web.Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Com.Coppel.Web.Api.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementación de repositorio para revisiones
/// </summary>
public class RevisionRepository : IRevisionRepository
{
    private readonly SpecDbContext _context;

    public RevisionRepository(SpecDbContext context)
    {
        _context = context;
    }

    public async Task<Revision?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Revisions
            .Include(r => r.Evaluations)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Revision?> GetByKeyAndNumberAsync(string key, int number, CancellationToken cancellationToken = default)
    {
        return await _context.Revisions
            .Include(r => r.Evaluations)
            .FirstOrDefaultAsync(r => r.Key == key && r.Number == number, cancellationToken);
    }

    public async Task<int> GetNextNumberAsync(string key, CancellationToken cancellationToken = default)
    {
        var maxNumber = await _context.Revisions
            .Where(r => r.Key == key)
            .MaxAsync(r => (int?)r.Number, cancellationToken);
        
        return (maxNumber ?? 0) + 1;
    }

    public async Task<Revision> AddAsync(Revision revision, CancellationToken cancellationToken = default)
    {
        await _context.Revisions.AddAsync(revision, cancellationToken);
        return revision;
    }

    public Task UpdateAsync(Revision revision, CancellationToken cancellationToken = default)
    {
        _context.Revisions.Update(revision);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Revision>> GetByTypeAsync(string type, CancellationToken cancellationToken = default)
    {
        return await _context.Revisions
            .Where(r => r.Type == type)
            .OrderByDescending(r => r.UploadedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Revision>> SearchAsync(
        string? searchTerm,
        string? type,
        DateTime? fromDate,
        DateTime? toDate,
        int page,
        int pageSize,
        string orderBy,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Revisions.AsQueryable();

        // Filtrar por término de búsqueda
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(r => 
                r.Name.Contains(searchTerm) || 
                r.Key.Contains(searchTerm));
        }

        // Filtrar por tipo
        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(r => r.Type == type);
        }

        // Filtrar por rango de fechas
        if (fromDate.HasValue)
        {
            query = query.Where(r => r.UploadedAt >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(r => r.UploadedAt <= toDate.Value);
        }

        // Ordenar
        query = orderBy.ToLowerInvariant() switch
        {
            "asc" => query.OrderBy(r => r.UploadedAt),
            _ => query.OrderByDescending(r => r.UploadedAt)
        };

        // Paginación
        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(r => r.Evaluations)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        string? searchTerm,
        string? type,
        DateTime? fromDate,
        DateTime? toDate,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Revisions.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(r => 
                r.Name.Contains(searchTerm) || 
                r.Key.Contains(searchTerm));
        }

        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(r => r.Type == type);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(r => r.UploadedAt >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(r => r.UploadedAt <= toDate.Value);
        }

        return await query.CountAsync(cancellationToken);
    }
}

