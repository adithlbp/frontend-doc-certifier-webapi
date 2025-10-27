using Com.Coppel.Web.Api.Core.Application.DTOs;
using Com.Coppel.Web.Api.Core.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Com.Coppel.Web.Api.Core.Application.UseCases.Revisions.Queries.GetRevisions;

/// <summary>
/// Handler para obtener lista de revisiones
/// </summary>
public class GetRevisionsQueryHandler
{
    private readonly IRevisionRepository _revisionRepository;
    private readonly IEvaluationRepository _evaluationRepository;
    private readonly ILogger<GetRevisionsQueryHandler> _logger;

    public GetRevisionsQueryHandler(
        IRevisionRepository revisionRepository,
        IEvaluationRepository evaluationRepository,
        ILogger<GetRevisionsQueryHandler> logger)
    {
        _revisionRepository = revisionRepository;
        _evaluationRepository = evaluationRepository;
        _logger = logger;
    }

    public async Task<RevisionListDto> Handle(GetRevisionsQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obteniendo lista de revisiones. Page: {Page}, PageSize: {PageSize}", 
            query.Page, query.PageSize);

        var revisions = await _revisionRepository.SearchAsync(
            query.SearchTerm,
            null,
            query.From,
            query.To,
            query.Page,
            query.PageSize,
            query.Order,
            cancellationToken);

        var totalItems = await _revisionRepository.CountAsync(
            query.SearchTerm,
            null,
            query.From,
            query.To,
            cancellationToken);

        var totalPages = (int)Math.Ceiling(totalItems / (double)query.PageSize);

        // Obtener evaluaciones asociadas
        var revisionIds = revisions.Select(r => r.Id).ToList();
        var evaluations = await _evaluationRepository.GetByRevisionIdsAsync(revisionIds, cancellationToken);
        var evaluationsByRevisionId = evaluations.ToDictionary(e => e.RevisionId);

        var items = revisions.Select(r =>
        {
            var evaluation = evaluationsByRevisionId.TryGetValue(r.Id, out var eval) ? eval : null;
            
            return new RevisionSummaryDto
            {
                RevisionId = r.Id,
                Number = r.Number,
                Type = r.Type,
                Key = r.Key,
                Name = r.Name,
                UploadedAt = r.UploadedAt,
                Checksum = r.Checksum ?? string.Empty,
                SpecMetadata = r.SpecVersion != null ? new SpecMetadataDto
                {
                    Version = r.SpecVersion,
                    Date = r.SpecDate,
                    Author = r.SpecAuthor ?? string.Empty
                } : null,
                Evaluation = evaluation != null ? new EvaluationSummaryDto
                {
                    Id = evaluation.Id,
                    OverallStatus = evaluation.OverallStatus ?? "PENDING"
                } : null
            };
        }).ToList();

        return new RevisionListDto
        {
            PageInfo = new PageInfoDto
            {
                Page = query.Page,
                PageSize = query.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            },
            Items = items
        };
    }
}

