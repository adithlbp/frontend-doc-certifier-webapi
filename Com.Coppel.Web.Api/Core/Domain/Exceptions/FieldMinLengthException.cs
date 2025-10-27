namespace Com.Coppel.Web.Api.Core.Domain.Exceptions
{
    /// <summary>
    /// Excepción para un campo con longitud mínima.
    /// </summary>
    public class FieldMinLengthException : DomainException
    {
        /// <summary>
        /// Constructor de la excepción FieldMinLengthException.
        /// </summary>
        /// <param name="field">Nombre del campo.</param>
        /// <param name="minLength">Longitud mínima requerida.</param>
        public FieldMinLengthException(string field, int minLength) : base($"El campo {field} debe tener al menos {minLength} caracteres.", "field_min_length") { }
    }
}