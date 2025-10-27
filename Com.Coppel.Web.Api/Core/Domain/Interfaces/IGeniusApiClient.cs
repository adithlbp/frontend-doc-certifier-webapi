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
    public string DocumentGeneral { get; set; } = string.Empty;
    public string DocumentServicio { get; set; } = string.Empty;
    public string? Model { get; set; }
    public string? PromptVersion { get; set; }
}

/// <summary>
/// Response de Genius API
/// </summary>
public class GeniusEvaluationResponse
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public string? ResultJson { get; set; }
    public TokenUsage? TokenUsage { get; set; }
}

public class TokenUsage
{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
}

