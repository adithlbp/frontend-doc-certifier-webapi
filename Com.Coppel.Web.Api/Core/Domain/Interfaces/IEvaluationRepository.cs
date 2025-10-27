using Com.Coppel.Web.Api.Core.Domain.Entities;

namespace Com.Coppel.Web.Api.Core.Domain.Interfaces;

/// <summary>
/// Repositorio para evaluaciones
/// </summary>
public interface IEvaluationRepository
{
    Task<Evaluation?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    
    Task<Evaluation?> GetByRevisionIdAsync(string revisionId, CancellationToken cancellationToken = default);
    
    Task<Evaluation?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default);
    
    Task<Evaluation> AddAsync(Evaluation evaluation, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(Evaluation evaluation, CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    
    Task<List<Evaluation>> GetByRevisionIdsAsync(List<string> revisionIds, CancellationToken cancellationToken = default);
}

