using Com.Coppel.Web.Api.Core.Application.DTOs;

namespace Com.Coppel.Web.Api.Core.Application.UseCases.Evaluations.Commands.CreateEvaluation;

/// <summary>
/// Command para crear una evaluación
/// </summary>
public class CreateEvaluationCommand
{
    public UploadDocumentsDto Documents { get; set; } = null!;
    public string? IdempotencyKey { get; set; }
}

