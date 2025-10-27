namespace Com.Coppel.Web.Api.Core.Application.DTOs
{
    /// <summary>
    /// DTO para actualizar un producto existente
    /// </summary>
    public class UpdateProductoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
    }
}