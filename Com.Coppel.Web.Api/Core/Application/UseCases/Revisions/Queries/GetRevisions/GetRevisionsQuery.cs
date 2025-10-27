namespace Com.Coppel.Web.Api.Core.Application.UseCases.Revisions.Queries.GetRevisions;

/// <summary>
/// Query para obtener lista paginada de revisiones
/// </summary>
public class GetRevisionsQuery
{
    public string? SearchTerm { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string Order { get; set; } = "desc";

    public GetRevisionsQuery(
        string? searchTerm = null,
        DateTime? from = null,
        DateTime? to = null,
        int page = 1,
        int pageSize = 50,
        string order = "desc")
    {
        SearchTerm = searchTerm;
        From = from;
        To = to;
        Page = page;
        PageSize = pageSize;
        Order = order;
    }
}

