using Com.Coppel.Web.Api.Core.Application.DTOs;
using Com.Coppel.Web.Api.Core.Domain.Entities;
using Com.Coppel.Web.Api.Core.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Com.Coppel.Web.Api.Core.Application.UseCases.Evaluations.Queries.GetEvaluation;

/// <summary>
/// Handler para obtener evaluación por ID
/// </summary>
public class GetEvaluationQueryHandler
{
    private readonly IEvaluationRepository _evaluationRepository;
    private readonly ILogger<GetEvaluationQueryHandler> _logger;

    public GetEvaluationQueryHandler(
        IEvaluationRepository evaluationRepository,
        ILogger<GetEvaluationQueryHandler> logger)
    {
        _evaluationRepository = evaluationRepository;
        _logger = logger;
    }

    public async Task<EvaluationDto?> Handle(GetEvaluationQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obteniendo evaluación por ID: {EvaluationId}", query.EvaluationId);

        var evaluation = await _evaluationRepository.GetByIdAsync(query.EvaluationId, cancellationToken);

        if (evaluation == null)
        {
            _logger.LogWarning("Evaluación no encontrada: {EvaluationId}", query.EvaluationId);
            return null;
        }

        return MapToDto(evaluation);
    }

    private EvaluationDto MapToDto(Evaluation evaluation)
    {
        return new EvaluationDto
        {
            EvaluationId = evaluation.Id,
            Status = evaluation.Status,
            CreatedAt = evaluation.CreatedAt,
            CompletedAt = evaluation.CompletedAt,
            Run = evaluation.Model != null ? new RunInfoDto
            {
                Model = evaluation.Model,
                PromptVersion = evaluation.PromptVersion ?? "v0.3",
                Retries = evaluation.Retries,
                Degraded = false
            } : null,
            Document = evaluation.Revision != null ? new DocumentInfoDto
            {
                Id = evaluation.Revision.Id,
                Name = evaluation.Revision.Name,
                Checksum = evaluation.Revision.Checksum ?? string.Empty,
                Revision = new RevisionInfoDto
                {
                    Number = evaluation.Revision.Number,
                    UploadedAt = evaluation.Revision.UploadedAt,
                    Source = "upload"
                },
                SpecMetadata = evaluation.Revision.SpecVersion != null ? new SpecMetadataDto
                {
                    Version = evaluation.Revision.SpecVersion,
                    Date = evaluation.Revision.SpecDate,
                    Author = evaluation.Revision.SpecAuthor ?? string.Empty
                } : null
            } : null,
            Scope = new ScopeDto
            {
                AppliedCriteriaCount = evaluation.ScoreTotal
            },
            Result = evaluation.OverallStatus != null ? new ResultDto
            {
                OverallStatus = evaluation.OverallStatus,
                Scores = new ScoresDto
                {
                    ByStatus = new ScoreByStatusDto
                    {
                        Pass = evaluation.ScorePass,
                        Fail = evaluation.ScoreFail,
                        Na = evaluation.ScoreNa,
                        Total = evaluation.ScoreTotal
                    },
                    BySeverity = new Dictionary<string, SeverityScoreDto>(),
                    Critical = new CriticalScoreDto
                    {
                        Passed = evaluation.CritPass,
                        Failed = evaluation.CritFail,
                        Na = evaluation.CritNa,
                        Ids = new CriticalIdsDto
                        {
                            Passed = new List<string>(),
                            Failed = new List<string>(),
                            Na = new List<string>()
                        }
                    }
                },
                Gate = new GateDto
                {
                    Rule = "no_critical_fail",
                    Passed = evaluation.GatePassed,
                    Reason = evaluation.GateReason ?? string.Empty,
                    Details = new GateDetailsDto
                    {
                        CriticalFailedCount = evaluation.CritFail,
                        FailedIds = new List<string>()
                    }
                },
                Criteria = evaluation.Criteria.Select(c => new CriterionDto
                {
                    Id = c.Criterion.Id,
                    Category = c.Criterion.Category,
                    Title = c.Criterion.Title,
                    Applicable = c.Status != "N/A",
                    Status = c.Status,
                    Severity = c.Severity,
                    Evidence = c.Evidence,
                    Comments = c.Comments
                }).ToList()
            } : null
        };
    }
}

