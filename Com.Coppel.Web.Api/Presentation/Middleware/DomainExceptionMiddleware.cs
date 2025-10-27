using System.Net;
using Com.Coppel.Web.Api.Core.Application.Common;
using Com.Coppel.Web.Api.Core.Application.Common.Logging;
using Com.Coppel.Web.Api.Core.Domain.Exceptions;
using Com.Coppel.Web.Api.Presentation.Common;

namespace Com.Coppel.Web.Api.Presentation.Middleware
{
    /// <summary>
    /// Middleware para manejar excepciones de dominio y excepciones generales, enviando respuestas estructuradas al cliente.
    /// </summary>
    public class DomainExceptionMiddleware : IMiddleware
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<DomainExceptionMiddleware> _logger;
        private readonly TraceIdentifier _traceIdentifier;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="DomainExceptionMiddleware"/>.
        /// </summary>
        public DomainExceptionMiddleware(IWebHostEnvironment env, ILogger<DomainExceptionMiddleware> logger, TraceIdentifier traceIdentifier)
        {
            _env = env;
            _logger = logger;
            _traceIdentifier = traceIdentifier;
        }

        /// <summary>
        /// Método invocado por el pipeline de procesamiento de solicitudes para manejar excepciones.
        /// </summary>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (DomainException e)
            {
                await HandleExceptionAsync(context, e, HttpStatusCode.BadRequest, e.Message, e.DevMessage);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e, HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.", e.Message);
            }
        }

        /// <summary>
        /// Maneja las excepciones capturadas, registrando los detalles y enviando una respuesta estructurada al cliente.
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode, string userMessage, string? devMessage)
        {
            var response = new FailureApiResponse(
                transactionID: _traceIdentifier.Id,
                statusCode: statusCode,
                message: userMessage,
                devMessage: _env.IsProduction() ? null : devMessage
            );

            DomainExceptionLogMessages.ErrorEnProcesamiento(_logger, context.Request.Path, response, exception);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}