namespace Com.Coppel.Web.Api.Core.Domain.Exceptions
{
    /// <summary>
    /// Excepción para una solicitud incorrecta.
    /// </summary>
    public class BadRequestException : DomainException
    {
        /// <summary>
        /// Constructor de la excepción BadRequestException.
        /// </summary>
        /// <param name="message">Mensaje de la excepción.</param>
        public BadRequestException(string message) : base(message, "bad_request") { }
    }
}