namespace Com.Coppel.Web.Api.Core.Domain.Exceptions
{
    /// <summary>
    /// Excepción para un método no encontrado.
    /// </summary>
    public class MethodNotFoundException : DomainException
    {
        /// <summary>
        /// Constructor de la excepción MethodNotFoundException.
        /// </summary>
        /// <param name="message">Mensaje de la excepción.</param>
        public MethodNotFoundException(string message) : base(message, "method_not_found") { }
    }
}