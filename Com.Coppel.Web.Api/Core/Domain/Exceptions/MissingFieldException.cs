namespace Com.Coppel.Web.Api.Core.Domain.Exceptions
{
    /// <summary>
    /// Excepción para un campo faltante.
    /// </summary>
    public class MissingFieldException : DomainException
    {
        /// <summary>
        /// Constructor de la excepción MissingFieldException.
        /// </summary>
        /// <param name="field">Nombre del campo faltante.</param>
        public MissingFieldException(string field) : base($"El campo {field} es requerido.", "missing_field") { }
    }
}