namespace Com.Coppel.Web.Api.Core.Application.UseCases.Evaluations.Queries.GetEvaluation;

/// <summary>
/// Query para obtener una evaluación por ID
/// </summary>
public class GetEvaluationQuery
{
    public string EvaluationId { get; set; } = string.Empty;

    public GetEvaluationQuery(string evaluationId)
    {
        EvaluationId = evaluationId;
    }
}

