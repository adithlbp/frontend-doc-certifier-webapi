using Com.Coppel.Web.Api.Core.Domain.Common;

namespace Com.Coppel.Web.Api.Core.Domain.Entities;

/// <summary>
/// Criterio de evaluación (catalogo)
/// </summary>
public class Criterion : BaseEntity
{
    /// <summary>
    /// Versión del criterio
    /// </summary>
    public string Version { get; set; } = string.Empty;
    
    public string Category { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Severidad: LOW|MEDIUM|CRITICAL
    /// </summary>
    public string Severity { get; set; } = "MEDIUM";
}

