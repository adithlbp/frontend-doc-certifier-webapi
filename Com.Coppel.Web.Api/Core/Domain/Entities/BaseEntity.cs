namespace Com.Coppel.Web.Api.Core.Domain.Entities
{
    /// <summary>
    /// Clase abstracta que representa una entidad base en el sistema.
    /// </summary>
    /// <typeparam name="TId">Tipo genérico para el identificador de la entidad.</typeparam>
    public abstract class BaseEntity<TId> where TId : struct
    {
        /// <summary>
        /// Propiedad que representa el identificador de la entidad.
        /// </summary>
        public TId Id { get; set; }

        /// <summary>
        /// Constructor sin parámetros que establece el valor predeterminado para la propiedad Id.
        /// </summary>
        protected BaseEntity()
        {
            Id = default;
        }

        /// <summary>
        /// Constructor que acepta un parámetro para establecer el valor de la propiedad Id.
        /// </summary>
        /// <param name="id">Valor del identificador de la entidad.</param>
        protected BaseEntity(TId id)
        {
            Id = id;
        }
    }
}