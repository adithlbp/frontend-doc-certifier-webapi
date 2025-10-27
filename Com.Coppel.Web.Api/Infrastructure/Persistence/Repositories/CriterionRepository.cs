using Com.Coppel.Web.Api.Core.Domain.Entities;
using Com.Coppel.Web.Api.Core.Domain.Interfaces;
using Com.Coppel.Web.Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Com.Coppel.Web.Api.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementación de repositorio para criterios
/// </summary>
public class CriterionRepository : ICriterionRepository
{
    private readonly SpecDbContext _context;

    public CriterionRepository(SpecDbContext context)
    {
        _context = context;
    }

    public async Task<Criterion?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Criteria
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<List<Criterion>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Criteria
            .OrderBy(c => c.Severity)
            .ThenBy(c => c.Category)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Criterion>> GetBySeverityAsync(string severity, CancellationToken cancellationToken = default)
    {
        return await _context.Criteria
            .Where(c => c.Severity == severity)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Criterion>> GetActiveVersionAsync(CancellationToken cancellationToken = default)
    {
        // Obtiene la versión más reciente
        var latestVersion = await _context.Criteria
            .MaxAsync(c => c.Version, cancellationToken);

        return await _context.Criteria
            .Where(c => c.Version == latestVersion)
            .ToListAsync(cancellationToken);
    }
}

