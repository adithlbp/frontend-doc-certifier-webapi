using Com.Coppel.Web.Api.Core.Domain.Entities;
using Com.Coppel.Web.Api.Core.Domain.Interfaces;
using Com.Coppel.Web.Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Com.Coppel.Web.Api.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementación de repositorio para evaluaciones
/// </summary>
public class EvaluationRepository : IEvaluationRepository
{
    private readonly SpecDbContext _context;

    public EvaluationRepository(SpecDbContext context)
    {
        _context = context;
    }

    public async Task<Evaluation?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Evaluations
            .Include(e => e.Revision)
            .Include(e => e.Criteria)
                .ThenInclude(ec => ec.Criterion)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<Evaluation?> GetByRevisionIdAsync(string revisionId, CancellationToken cancellationToken = default)
    {
        return await _context.Evaluations
            .Include(e => e.Revision)
            .Include(e => e.Criteria)
                .ThenInclude(ec => ec.Criterion)
            .FirstOrDefaultAsync(e => e.RevisionId == revisionId, cancellationToken);
    }

    public async Task<Evaluation?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default)
    {
        return await _context.Evaluations
            .Include(e => e.Revision)
            .Include(e => e.Criteria)
                .ThenInclude(ec => ec.Criterion)
            .FirstOrDefaultAsync(e => e.Idempotency == idempotencyKey, cancellationToken);
    }

    public async Task<Evaluation> AddAsync(Evaluation evaluation, CancellationToken cancellationToken = default)
    {
        await _context.Evaluations.AddAsync(evaluation, cancellationToken);
        return evaluation;
    }

    public Task UpdateAsync(Evaluation evaluation, CancellationToken cancellationToken = default)
    {
        _context.Evaluations.Update(evaluation);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Evaluation>> GetByRevisionIdsAsync(List<string> revisionIds, CancellationToken cancellationToken = default)
    {
        return await _context.Evaluations
            .Include(e => e.Revision)
            .Include(e => e.Criteria)
                .ThenInclude(ec => ec.Criterion)
            .Where(e => revisionIds.Contains(e.RevisionId))
            .ToListAsync(cancellationToken);
    }
}

