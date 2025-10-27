namespace Com.Coppel.Web.Api.Core.Domain.Exceptions
{
    /// <summary>
    /// Excepción para una entidad no encontrada.
    /// </summary>
    public class RecordNotFoundException : DomainException
    {
        /// <summary>
        /// Constructor de la excepción RecordNotFoundException.
        /// </summary>
        /// <param name="message">Mensaje de la excepción.</param>
        public RecordNotFoundException(string message) : base(message, "record_not_found") { }
    }

    /// <summary>
    /// Excepción para una entidad no encontrada con un identificador específico.
    /// </summary>
    /// <typeparam name="TId">Tipo de dato del identificador.</typeparam>
    public class RecordNotFoundException<TId> : RecordNotFoundException
    {
        /// <summary>
        /// Identificador de la entidad no encontrada.
        /// </summary>
        public TId Id { get; }

        /// <summary>
        /// Constructor de la excepción RecordNotFoundException.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        public RecordNotFoundException(TId id) : base($"No se encontró la entidad con el id {id}.")
        {
            Id = id;
        }
    }

}