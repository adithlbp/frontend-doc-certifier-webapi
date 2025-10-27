namespace Com.Coppel.Web.Api.Core.Domain.Exceptions
{
    /// <summary>
    /// Excepción para un campo con longitud máxima.
    /// </summary>
    public class FieldMaxLengthException : DomainException
    {
        /// <summary>
        /// Constructor de la excepción FieldMaxLengthException.
        /// </summary>
        /// <param name="field">Nombre del campo.</param>
        /// <param name="maxLength">Longitud máxima permitida.</param>
        public FieldMaxLengthException(string field, int maxLength) : base($"El campo {field} debe tener máximo {maxLength} caracteres.", "field_max_length") { }
    }
}