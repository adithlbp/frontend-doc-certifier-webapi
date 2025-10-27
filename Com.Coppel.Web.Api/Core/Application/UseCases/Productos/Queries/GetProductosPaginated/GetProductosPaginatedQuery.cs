using Com.Coppel.Web.Api.Core.Application.Common;

namespace Com.Coppel.Web.Api.Core.Application.UseCases.Productos.Queries.GetProductosPaginated
{
    /// <summary>
    /// Query para obtener productos paginados
    /// </summary>
    public class GetProductosPaginatedQuery
    {
        public PaginatedQueryParams QueryParams { get; set; }

        public GetProductosPaginatedQuery(PaginatedQueryParams queryParams)
        {
            QueryParams = queryParams;
        }
    }
}