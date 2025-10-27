namespace Com.Coppel.Web.Api.Core.Application.DTOs;

/// <summary>
/// DTO para resumen de revisión (lista)
/// </summary>
public class RevisionSummaryDto
{
    public string RevisionId { get; set; } = string.Empty;
    public int Number { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public string Checksum { get; set; } = string.Empty;
    
    /// <summary>
    /// Contenido del markdown
    /// </summary>
    public string? Content { get; set; }
    public SpecMetadataDto? SpecMetadata { get; set; }
    public EvaluationSummaryDto? Evaluation { get; set; }
}

public class EvaluationSummaryDto
{
    public string Id { get; set; } = string.Empty;
    public string OverallStatus { get; set; } = "PENDING";
}

/// <summary>
/// DTO para detalle completo de revisión
/// </summary>
public class RevisionDetailDto : RevisionSummaryDto
{
    public EvaluationDetailDto? Evaluation { get; set; }
}

public class EvaluationDetailDto : EvaluationSummaryDto
{
    public ScoresDto? Scores { get; set; }
    public GateDto? Gate { get; set; }
}

/// <summary>
/// DTO para lista paginada de revisiones
/// </summary>
public class RevisionListDto
{
    public PageInfoDto PageInfo { get; set; } = new();
    public List<RevisionSummaryDto> Items { get; set; } = new();
}

public class PageInfoDto
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}

