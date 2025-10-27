using System.Net;

namespace Com.Coppel.Web.Api.Core.Domain.Exceptions
{
    /// <summary>
    /// Excepción genérica para errores al consumir una API.
    /// </summary>
    public class GenericApiException : DomainException
    {
        /// <summary>
        /// Mensaje de respuesta HTTP.
        /// </summary>
        public HttpResponseMessage HttpResponseMessage { get; }

        /// <summary>
        /// Código de estado HTTP.
        /// </summary>
        public HttpStatusCode StatusCode => HttpResponseMessage.StatusCode;

        /// <summary>
        /// Constructor de la excepción GenericApiException.
        /// </summary>
        /// <param name="httpResponseMessage">Respuesta HTTP.</param>
        public GenericApiException(HttpResponseMessage httpResponseMessage) : base($"Error al consumir el API.", "generic_api_error")
        {
            HttpResponseMessage = httpResponseMessage;
        }
    }
}