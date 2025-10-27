using System.ComponentModel.DataAnnotations;

namespace Com.Coppel.Web.Api.Core.Application.DTOs;

/// <summary>
/// DTO para subir documentos para evaluación
/// </summary>
public class UploadDocumentsDto
{
    /// <summary>
    /// Archivo Markdown (.md) de Especificación General (obligatorio)
    /// </summary>
    [Required]
    public IFormFile FileGeneral { get; set; } = null!;
    
    /// <summary>
    /// Archivo Markdown (.md) de Especificación de Frontend (obligatorio)
    /// </summary>
    [Required]
    public IFormFile FileServicio { get; set; } = null!;
    
    /// <summary>
    /// Clave de idempotencia para evitar duplicados (opcional)
    /// </summary>
    public string? IdempotencyKey { get; set; }
}

