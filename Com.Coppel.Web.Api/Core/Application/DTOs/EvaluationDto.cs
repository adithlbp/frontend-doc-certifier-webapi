using System.Text.Json;

namespace Com.Coppel.Web.Api.Core.Application.DTOs;

/// <summary>
/// DTO para respuesta de evaluación
/// </summary>
public class EvaluationDto
{
    public string EvaluationId { get; set; } = string.Empty;
    
    public string Status { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }
    
    public RunInfoDto? Run { get; set; }
    
    public DocumentInfoDto? Document { get; set; }
    
    public ScopeDto? Scope { get; set; }
    
    public ResultDto? Result { get; set; }
    
    public ArtifactsDto? Artifacts { get; set; }
    
    public AuditDto? Audit { get; set; }
    
    public ErrorDto? Error { get; set; }
}

public class RunInfoDto
{
    public string Model { get; set; } = string.Empty;
    public string PromptVersion { get; set; } = string.Empty;
    public int Retries { get; set; }
    public bool Degraded { get; set; }
}

public class DocumentInfoDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Checksum { get; set; } = string.Empty;
    
    /// <summary>
    /// Contenido extraído del markdown
    /// </summary>
    public string? Content { get; set; }
    public RevisionInfoDto? Revision { get; set; }
    public SpecMetadataDto? SpecMetadata { get; set; }
}

public class RevisionInfoDto
{
    public int Number { get; set; }
    public DateTime UploadedAt { get; set; }
    public string Source { get; set; } = "upload";
}

public class SpecMetadataDto
{
    public string Version { get; set; } = string.Empty;
    public DateOnly? Date { get; set; }
    public string Author { get; set; } = string.Empty;
}

public class ScopeDto
{
    public int AppliedCriteriaCount { get; set; }
}

public class ResultDto
{
    public string OverallStatus { get; set; } = string.Empty;
    public ScoresDto Scores { get; set; } = new();
    public GateDto? Gate { get; set; }
    public List<CriterionDto> Criteria { get; set; } = new();
}

public class ScoresDto
{
    public ScoreByStatusDto ByStatus { get; set; } = new();
    public Dictionary<string, SeverityScoreDto> BySeverity { get; set; } = new();
    public CriticalScoreDto Critical { get; set; } = new();
}

public class ScoreByStatusDto
{
    public int Pass { get; set; }
    public int Fail { get; set; }
    public int Na { get; set; }
    public int Total { get; set; }
}

public class SeverityScoreDto
{
    public int Pass { get; set; }
    public int Fail { get; set; }
    public int Na { get; set; }
    public int Total { get; set; }
}

public class CriticalScoreDto
{
    public int Passed { get; set; }
    public int Failed { get; set; }
    public int Na { get; set; }
    public CriticalIdsDto Ids { get; set; } = new();
}

public class CriticalIdsDto
{
    public List<string> Passed { get; set; } = new();
    public List<string> Failed { get; set; } = new();
    public List<string> Na { get; set; } = new();
}

public class GateDto
{
    public string Rule { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public string Reason { get; set; } = string.Empty;
    public GateDetailsDto? Details { get; set; }
}

public class GateDetailsDto
{
    public int CriticalFailedCount { get; set; }
    public List<string> FailedIds { get; set; } = new();
}

public class CriterionDto
{
    public string Id { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool Applicable { get; set; }
    public string Status { get; set; } = "N/A";
    public string Severity { get; set; } = "MEDIUM";
    public string? Evidence { get; set; }
    public string? Comments { get; set; }
}

public class ArtifactsDto
{
    public string? ReportPdfUrl { get; set; }
    public string? RawJsonUrl { get; set; }
}

public class AuditDto
{
    public string? InputsHash { get; set; }
    public int? DocChunksUsed { get; set; }
    public TokenUsageDto? Tokens { get; set; }
    public decimal? CostApproxUsd { get; set; }
}

public class TokenUsageDto
{
    public int In { get; set; }
    public int Out { get; set; }
}

public class ErrorDto
{
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

