using Com.Coppel.Web.Api.Core.Domain.Entities;

namespace Com.Coppel.Web.Api.Core.Domain.Interfaces;

/// <summary>
/// Repositorio para criterios (catalogo)
/// </summary>
public interface ICriterionRepository
{
    Task<Criterion?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    
    Task<List<Criterion>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<List<Criterion>> GetBySeverityAsync(string severity, CancellationToken cancellationToken = default);
    
    Task<List<Criterion>> GetActiveVersionAsync(CancellationToken cancellationToken = default);
}

