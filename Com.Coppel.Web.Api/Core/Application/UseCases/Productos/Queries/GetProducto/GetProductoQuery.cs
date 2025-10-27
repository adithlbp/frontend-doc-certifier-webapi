namespace Com.Coppel.Web.Api.Core.Application.UseCases.Productos.Queries.GetProducto
{
    /// <summary>
    /// Query para obtener un producto por ID
    /// </summary>
    public class GetProductoQuery
    {
        public Guid Id { get; set; }

        public GetProductoQuery(Guid id)
        {
            Id = id;
        }
    }
}