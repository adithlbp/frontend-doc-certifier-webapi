using Com.Coppel.Web.Api.Core.Application.DTOs;
using Com.Coppel.Web.Api.Core.Application.UseCases.Revisions.Queries.GetRevisions;
using Com.Coppel.Web.Api.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Com.Coppel.Web.Api.Presentation.Controllers;

/// <summary>
/// Controlador para gestionar revisiones de documentos
/// </summary>
[ApiController]
[Route("v1/revisions")]
public class RevisionsController : ControllerBase
{
    private readonly GetRevisionsQueryHandler _getRevisionsHandler;
    private readonly TraceIdentifier _traceIdentifier;
    private readonly ILogger<RevisionsController> _logger;

    public RevisionsController(
        GetRevisionsQueryHandler getRevisionsHandler,
        TraceIdentifier traceIdentifier,
        ILogger<RevisionsController> logger)
    {
        _getRevisionsHandler = getRevisionsHandler;
        _traceIdentifier = traceIdentifier;
        _logger = logger;
    }

    /// <summary>
    /// Listar historial de revisiones (paginado y con búsqueda)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(RevisionListDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<RevisionListDto>> GetRevisions(
        [FromQuery] string? nombre = null,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string order = "desc",
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Obteniendo lista de revisiones. Page: {Page}", page);

        var query = new GetRevisionsQuery(
            nombre,
            from,
            to,
            page,
            pageSize,
            order);

        var result = await _getRevisionsHandler.Handle(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Obtener detalle de una revisión
    /// </summary>
    [HttpGet("{revisionId}")]
    [ProducesResponseType(typeof(RevisionDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RevisionDetailDto>> GetRevision(
        [FromRoute] string revisionId,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implementar query handler para detalle
        _logger.LogWarning("GetRevision por ID no implementado aún");
        return NotFound();
    }
}

