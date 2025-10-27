using Com.Coppel.Web.Api.Core.Domain.Common;

namespace Com.Coppel.Web.Api.Core.Domain.Entities;

/// <summary>
/// Criterio evaluado en una evaluación específica
/// </summary>
public class EvaluationCriterion : BaseEntity<string>
{
    public string EvaluationId { get; set; } = string.Empty;
    
    public string CriterionId { get; set; } = string.Empty;
    
    /// <summary>
    /// Estado: PASS|FAIL|N/A
    /// </summary>
    public string Status { get; set; } = "N/A";
    
    public string Severity { get; set; } = "MEDIUM";
    
    /// <summary>
    /// Evidencia encontrada
    /// </summary>
    public string? Evidence { get; set; }
    
    /// <summary>
    /// Comentarios adicionales
    /// </summary>
    public string? Comments { get; set; }
    
    // Navigation
    public Evaluation Evaluation { get; set; } = null!;
    public Criterion Criterion { get; set; } = null!;
}

