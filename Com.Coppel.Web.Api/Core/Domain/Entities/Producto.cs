using System.ComponentModel.DataAnnotations;

using Com.Coppel.Web.Api.Core.Domain.Common;

namespace Com.Coppel.Web.Api.Core.Domain.Entities
{
    public class Producto : BaseEntity<Guid>
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Searchable]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        [Searchable]
        public required string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(1, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0.")]
        public required decimal PrecioUnitario { get; set; }

        /// <summary>
        /// Constructor para Entity Framework
        /// </summary>
        public Producto() { }

        /// <summary>
        /// Crea una nueva instancia de Producto
        /// </summary>
        public static Producto Create(string nombre, string descripcion, decimal precioUnitario)
        {
            var producto = new Producto
            {
                Id = Guid.NewGuid(),
                Nombre = nombre,
                Descripcion = descripcion,
                PrecioUnitario = precioUnitario
            };

            producto.ValidateInvariants();
            return producto;
        }

        /// <summary>
        /// Actualiza el precio del producto
        /// </summary>
        public void UpdatePrice(decimal nuevoPrecio)
        {
            if (nuevoPrecio <= 0)
                throw new ArgumentException("El precio debe ser mayor a cero");

            PrecioUnitario = nuevoPrecio;
        }

        /// <summary>
        /// Valida las reglas de negocio del producto
        /// </summary>
        private void ValidateInvariants()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
                throw new ArgumentException("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(Descripcion))
                throw new ArgumentException("La descripción es requerida");

            if (PrecioUnitario <= 0)
                throw new ArgumentException("El precio debe ser mayor a cero");
        }
    }
}