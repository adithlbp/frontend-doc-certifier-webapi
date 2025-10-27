using Com.Coppel.Web.Api.Core.Domain.Common;

namespace Com.Coppel.Web.Api.Core.Domain.Entities;

/// <summary>
/// Revisión de documento (general o frontend)
/// </summary>
public class Revision : BaseEntity
{
    public int Number { get; set; }
    
    /// <summary>
    /// Tipo de revisión: 'general' o 'frontend'
    /// </summary>
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// Slug lógico (ej: "especificacion-general")
    /// </summary>
    public string Key { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public DateTime UploadedAt { get; set; }
    
    /// <summary>
    /// Versión del spec extraída del documento
    /// </summary>
    public string? SpecVersion { get; set; }
    
    /// <summary>
    /// Fecha del spec
    /// </summary>
    public DateOnly? SpecDate { get; set; }
    
    /// <summary>
    /// Autor del spec
    /// </summary>
    public string? SpecAuthor { get; set; }
    
    /// <summary>
    /// Checksum del archivo (sha256)
    /// </summary>
    public string? Checksum { get; set; }
    
    // Navigation
    public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
}

