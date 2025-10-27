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
    public OverallStatusDto OverallStatus { get; set; } = new();
    public ScoresDto Scores { get; set; } = new();
    public GateDto Gate { get; set; } = new();
    public List<CriterionResultDto> Criteria { get; set; } = new();
}

public class OverallStatusDto
{
    public string Status { get; set; } = "PENDING"; // PASS|FAIL|PARTIAL
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

