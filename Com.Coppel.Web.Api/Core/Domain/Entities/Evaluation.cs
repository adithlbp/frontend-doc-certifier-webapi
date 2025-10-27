using Com.Coppel.Web.Api.Core.Domain.Common;
using System.Text.Json;

namespace Com.Coppel.Web.Api.Core.Domain.Entities;

/// <summary>
/// Evaluación de documentos con resultados de criterios
/// </summary>
public class Evaluation : BaseEntity
{
    /// <summary>
    /// ID de la revisión asociada
    /// </summary>
    public string RevisionId { get; set; } = string.Empty;
    
    /// <summary>
    /// Clave de idempotencia para evitar reprocesamiento
    /// </summary>
    public string? Idempotency { get; set; }
    
    /// <summary>
    /// Estado: PENDING|RUNNING|DONE|ERROR
    /// </summary>
    public string Status { get; set; } = "PENDING";
    
    /// <summary>
    /// Estado general: PASS|FAIL|PARTIAL
    /// </summary>
    public string? OverallStatus { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }
    
    /// <summary>
    /// Modelo LLM usado (ej: gemini-1.5-pro)
    /// </summary>
    public string? Model { get; set; }
    
    /// <summary>
    /// Versión del prompt usado
    /// </summary>
    public string? PromptVersion { get; set; }
    
    /// <summary>
    /// Intentos realizados
    /// </summary>
    public int Retries { get; set; }
    
    // Scores
    public int ScorePass { get; set; }
    public int ScoreFail { get; set; }
    public int ScoreNa { get; set; }
    public int ScoreTotal { get; set; }
    
    // Criterios críticos
    public int CritPass { get; set; }
    public int CritFail { get; set; }
    public int CritNa { get; set; }
    
    // Gate
    public bool GatePassed { get; set; }
    public string? GateReason { get; set; }
    
    /// <summary>
    /// Resultado completo en JSON
    /// </summary>
    public JsonDocument? RawResult { get; set; }
    
    // Navigation
    public Revision Revision { get; set; } = null!;
    public ICollection<EvaluationCriterion> Criteria { get; set; } = new List<EvaluationCriterion>();
}

