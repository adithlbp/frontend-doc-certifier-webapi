namespace Com.Coppel.Web.Api.Core.Application.DTOs
{
    /// <summary>
    /// DTO para transferencia de datos de Producto
    /// </summary>
    public class ProductoDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
    }
}