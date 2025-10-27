namespace Com.Coppel.Web.Api.Core.Application.DTOs
{
    /// <summary>
    /// DTO para crear un nuevo producto
    /// </summary>
    public class CreateProductoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
    }
}