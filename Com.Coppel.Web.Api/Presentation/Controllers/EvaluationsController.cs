using Com.Coppel.Web.Api.Core.Application.DTOs;
using Com.Coppel.Web.Api.Core.Application.UseCases.Evaluations.Commands.CreateEvaluation;
using Com.Coppel.Web.Api.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Com.Coppel.Web.Api.Presentation.Controllers;

/// <summary>
/// Controlador para gestionar evaluaciones de documentos
/// </summary>
[ApiController]
[Route("v1/evaluations")]
public class EvaluationsController : ControllerBase
{
    private readonly CreateEvaluationCommandHandler _createHandler;
    private readonly TraceIdentifier _traceIdentifier;
    private readonly ILogger<EvaluationsController> _logger;

    public EvaluationsController(
        CreateEvaluationCommandHandler createHandler,
        TraceIdentifier traceIdentifier,
        ILogger<EvaluationsController> logger)
    {
        _createHandler = createHandler;
        _traceIdentifier = traceIdentifier;
        _logger = logger;
    }

    /// <summary>
    /// Crear evaluación (subiendo PDFs) y devolver resultado
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(EvaluationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<EvaluationDto>> CreateEvaluation(
        [FromForm] UploadDocumentsDto documents,
        [FromForm] string? idempotencyKey = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (documents.FileGeneral == null || documents.FileServicio == null)
            {
                return BadRequest(new { error = "file_general y file_servicio son requeridos" });
            }

            _logger.LogInformation("Creando evaluación de documentos");

            var command = new CreateEvaluationCommand
            {
                Documents = documents,
                IdempotencyKey = idempotencyKey
            };

            var result = await _createHandler.Handle(command, cancellationToken);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear evaluación");
            return StatusCode(500, new { error = "Error interno al procesar evaluación" });
        }
    }

    /// <summary>
    /// Obtener evaluación por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EvaluationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EvaluationDto>> GetEvaluation(
        [FromRoute] string id,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implementar query handler
        return NotFound();
    }
}

