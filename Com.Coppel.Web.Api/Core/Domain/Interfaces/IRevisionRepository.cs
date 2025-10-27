using Com.Coppel.Web.Api.Core.Domain.Entities;

namespace Com.Coppel.Web.Api.Core.Domain.Interfaces;

/// <summary>
/// Repositorio para revisiones
/// </summary>
public interface IRevisionRepository
{
    Task<Revision?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    
    Task<Revision?> GetByKeyAndNumberAsync(string key, int number, CancellationToken cancellationToken = default);
    
    Task<int> GetNextNumberAsync(string key, CancellationToken cancellationToken = default);
    
    Task<Revision> AddAsync(Revision revision, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(Revision revision, CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    
    Task<List<Revision>> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
    
    Task<List<Revision>> SearchAsync(
        string? searchTerm,
        string? type,
        DateTime? fromDate,
        DateTime? toDate,
        int page,
        int pageSize,
        string orderBy,
        CancellationToken cancellationToken = default);
    
    Task<int> CountAsync(
        string? searchTerm,
        string? type,
        DateTime? fromDate,
        DateTime? toDate,
        CancellationToken cancellationToken = default);
}

