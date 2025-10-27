using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Com.Coppel.Web.Api.Core.Application.DTOs;
using Com.Coppel.Web.Api.Core.Domain.Entities;
using Com.Coppel.Web.Api.Core.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Com.Coppel.Web.Api.Core.Application.UseCases.Evaluations.Commands.CreateEvaluation;

/// <summary>
/// Handler para crear evaluación de documentos
/// </summary>
public class CreateEvaluationCommandHandler
{
    private readonly IEvaluationRepository _evaluationRepository;
    private readonly IRevisionRepository _revisionRepository;
    private readonly IGeniusApiClient _geniusApiClient;
    private readonly ILogger<CreateEvaluationCommandHandler> _logger;

    public CreateEvaluationCommandHandler(
        IEvaluationRepository evaluationRepository,
        IRevisionRepository revisionRepository,
        IGeniusApiClient geniusApiClient,
        ILogger<CreateEvaluationCommandHandler> logger)
    {
        _evaluationRepository = evaluationRepository;
        _revisionRepository = revisionRepository;
        _geniusApiClient = geniusApiClient;
        _logger = logger;
    }

    public async Task<EvaluationDto> Handle(CreateEvaluationCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando evaluación de documentos");

        // 1. Verificar idempotencia
        if (!string.IsNullOrEmpty(command.IdempotencyKey))
        {
            var existingEvaluation = await _evaluationRepository
                .GetByIdempotencyKeyAsync(command.IdempotencyKey, cancellationToken);
            
            if (existingEvaluation != null)
            {
                _logger.LogInformation("Evaluación ya existe para idempotency key: {IdempotencyKey}", 
                    command.IdempotencyKey);
                return MapToDto(existingEvaluation);
            }
        }

        // 2. Procesar PDFs y extraer contenido
        var documentGeneralContent = await ExtractPdfContentAsync(command.Documents.FileGeneral, cancellationToken);
        var documentServicioContent = await ExtractPdfContentAsync(command.Documents.FileServicio, cancellationToken);

        // 3. Crear revisiones en la base de datos
        var generalKey = $"especificacion-general-{DateTime.UtcNow:yyyyMMdd}";
        var frontendKey = $"especificacion-frontend-{DateTime.UtcNow:yyyyMMdd}";
        
        var generalNumber = await _revisionRepository.GetNextNumberAsync(generalKey, cancellationToken);
        var frontendNumber = await _revisionRepository.GetNextNumberAsync(frontendKey, cancellationToken);

        var generalRevision = new Revision
        {
            Id = Guid.NewGuid().ToString(),
            Number = generalNumber,
            Type = "general",
            Key = generalKey,
            Name = command.Documents.FileGeneral.FileName,
            UploadedAt = DateTime.UtcNow,
            Checksum = CalculateChecksum(documentGeneralContent)
        };

        var frontendRevision = new Revision
        {
            Id = Guid.NewGuid().ToString(),
            Number = frontendNumber,
            Type = "frontend",
            Key = frontendKey,
            Name = command.Documents.FileServicio.FileName,
            UploadedAt = DateTime.UtcNow,
            Checksum = CalculateChecksum(documentServicioContent)
        };

        await _revisionRepository.AddAsync(generalRevision, cancellationToken);
        await _revisionRepository.AddAsync(frontendRevision, cancellationToken);
        await _revisionRepository.SaveChangesAsync(cancellationToken);

        // 4. Crear evaluación en estado RUNNING
        var evaluation = new Evaluation
        {
            Id = Guid.NewGuid().ToString(),
            RevisionId = frontendRevision.Id,
            Idempotency = command.IdempotencyKey,
            Status = "RUNNING",
            CreatedAt = DateTime.UtcNow,
            Model = "gemini-1.5-pro",
            PromptVersion = "v0.3",
            Retries = 0
        };

        await _evaluationRepository.AddAsync(evaluation, cancellationToken);
        await _evaluationRepository.SaveChangesAsync(cancellationToken);

        try
        {
            // 5. Llamar a Genius API
            var geniusRequest = new GeniusEvaluationRequest
            {
                DocumentGeneral = documentGeneralContent,
                DocumentServicio = documentServicioContent,
                Model = "gemini-1.5-pro",
                PromptVersion = "v0.3"
            };

            var geniusResponse = await _geniusApiClient.EvaluateDocumentsAsync(geniusRequest, cancellationToken);

            if (!geniusResponse.Success)
            {
                evaluation.Status = "ERROR";
                evaluation.CompletedAt = DateTime.UtcNow;
                _logger.LogError("Genius API retornó error: {Error}", geniusResponse.Error);
                
                await _evaluationRepository.UpdateAsync(evaluation, cancellationToken);
                await _evaluationRepository.SaveChangesAsync(cancellationToken);
                
                return MapToDto(evaluation);
            }

            // 6. Procesar resultado y calcular scores
            var resultJson = JsonDocument.Parse(geniusResponse.ResultJson ?? "{}");
            var scores = CalculateScores(resultJson);
            
            evaluation.Status = "DONE";
            evaluation.CompletedAt = DateTime.UtcNow;
            evaluation.OverallStatus = scores.GatePassed ? "PASS" : "FAIL";
            evaluation.ScorePass = scores.ScorePass;
            evaluation.ScoreFail = scores.ScoreFail;
            evaluation.ScoreNa = scores.ScoreNa;
            evaluation.ScoreTotal = scores.ScoreTotal;
            evaluation.CritPass = scores.CritPass;
            evaluation.CritFail = scores.CritFail;
            evaluation.CritNa = scores.CritNa;
            evaluation.GatePassed = scores.GatePassed;
            evaluation.GateReason = scores.GateReason;
            evaluation.RawResult = resultJson;

            await _evaluationRepository.UpdateAsync(evaluation, cancellationToken);
            await _evaluationRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Evaluación completada exitosamente");

            return MapToDto(evaluation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar evaluación");
            
            evaluation.Status = "ERROR";
            evaluation.CompletedAt = DateTime.UtcNow;
            
            await _evaluationRepository.UpdateAsync(evaluation, cancellationToken);
            await _evaluationRepository.SaveChangesAsync(cancellationToken);
            
            return MapToDto(evaluation);
        }
    }

    private static async Task<string> ExtractPdfContentAsync(IFormFile file, CancellationToken cancellationToken)
    {
        // TODO: Implementar extracción real de PDF usando iTextSharp
        // Por ahora retornamos contenido simulado
        using var stream = new StreamReader(file.OpenReadStream());
        return await stream.ReadToEndAsync();
    }

    private static string CalculateChecksum(string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var hash = SHA256.HashData(bytes);
        return $"sha256:{Convert.ToHexString(hash).ToLowerInvariant()}";
    }

    private static (int ScorePass, int ScoreFail, int ScoreNa, int ScoreTotal, int CritPass, int CritFail, int CritNa, bool GatePassed, string GateReason) 
        CalculateScores(JsonDocument resultJson)
    {
        // TODO: Implementar lógica real de cálculo de scores
        // Por ahora valores por defecto
        return (9, 4, 3, 16, 5, 2, 1, false, "Hay 2 criterios críticos en FAIL");
    }

    private static EvaluationDto MapToDto(Evaluation evaluation)
    {
        return new EvaluationDto
        {
            EvaluationId = evaluation.Id,
            Status = evaluation.Status,
            CreatedAt = evaluation.CreatedAt,
            CompletedAt = evaluation.CompletedAt,
            Run = evaluation.Model != null ? new RunInfoDto
            {
                Model = evaluation.Model,
                PromptVersion = evaluation.PromptVersion ?? "v0.3",
                Retries = evaluation.Retries,
                Degraded = false
            } : null
        };
    }
}

