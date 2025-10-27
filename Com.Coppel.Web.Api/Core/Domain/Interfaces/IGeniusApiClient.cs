namespace Com.Coppel.Web.Api.Core.Domain.Interfaces;

/// <summary>
/// Cliente para Genius API
/// </summary>
public interface IGeniusApiClient
{
    /// <summary>
    /// Envía documentos para evaluación con Genius
    /// </summary>
    Task<GeniusEvaluationResponse> EvaluateDocumentsAsync(GeniusEvaluationRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Request para Genius API
/// </summary>
public class GeniusEvaluationRequest
{
    public string DocumentGeneralMarkdown { get; set; } = string.Empty;
    public string DocumentServicioMarkdown { get; set; } = string.Empty;
    public string? Model { get; set; }
    public string? PromptVersion { get; set; }
}

/// <summary>
/// Response de Genius API - Formato JSON estructurado
/// </summary>
public class GeniusEvaluationResponse
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    
    /// <summary>
    /// Resultado en formato JSON estructurado con scores y criterios
    /// </summary>
    public EvaluationResultJson? Result { get; set; }
    
    public TokenUsage? TokenUsage { get; set; }
}

/// <summary>
/// Formato estructurado de respuesta de evaluación
/// </summary>
public class EvaluationResultJson
{
    public GeniusOverallStatusDto OverallStatus { get; set; } = new();
    public GeniusScoresDto Scores { get; set; } = new();
    public GeniusGateDto Gate { get; set; } = new();
    public List<CriterionResultDto> Criteria { get; set; } = new();
}

public class GeniusOverallStatusDto
{
    public string Status { get; set; } = "PENDING"; // PASS|FAIL|PARTIAL
}

public class GeniusScoresDto
{
    public GeniusScoreByStatusDto ByStatus { get; set; } = new();
    public Dictionary<string, GeniusSeverityScoreDto> BySeverity { get; set; } = new();
    public GeniusCriticalScoreDto Critical { get; set; } = new();
}

public class GeniusScoreByStatusDto
{
    public int Pass { get; set; }
    public int Fail { get; set; }
    public int Na { get; set; }
    public int Total { get; set; }
}

public class GeniusSeverityScoreDto
{
    public int Pass { get; set; }
    public int Fail { get; set; }
    public int Na { get; set; }
    public int Total { get; set; }
}

public class GeniusCriticalScoreDto
{
    public int Passed { get; set; }
    public int Failed { get; set; }
    public int Na { get; set; }
    public GeniusCriticalIdsDto Ids { get; set; } = new();
}

public class GeniusCriticalIdsDto
{
    public List<string> Passed { get; set; } = new();
    public List<string> Failed { get; set; } = new();
    public List<string> Na { get; set; } = new();
}

public class GeniusGateDto
{
    public string Rule { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public string Reason { get; set; } = string.Empty;
    public GeniusGateDetailsDto Details { get; set; } = new();
}

public class GeniusGateDetailsDto
{
    public int CriticalFailedCount { get; set; }
    public List<string> FailedIds { get; set; } = new();
}

public class CriterionResultDto
{
    public string Id { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool Applicable { get; set; }
    public string Status { get; set; } = "N/A"; // PASS|FAIL|N/A
    public string Severity { get; set; } = "MEDIUM";
    public string? Evidence { get; set; }
    public string? Comments { get; set; }
}

public class TokenUsage
{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
}

