namespace Com.Coppel.Web.Api.Core.Application.UseCases.Productos.Commands.CreateProducto
{
    /// <summary>
    /// Comando para crear un nuevo producto
    /// </summary>
    public class CreateProductoCommand
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
    }
}