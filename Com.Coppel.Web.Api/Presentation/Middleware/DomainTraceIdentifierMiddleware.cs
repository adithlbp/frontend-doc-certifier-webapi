using Com.Coppel.Web.Api.Presentation.Common;

namespace Com.Coppel.Web.Api.Presentation.Middleware
{
    /// <summary>
    /// Middleware para asignar un identificador de seguimiento personalizado a cada solicitud HTTP.
    /// </summary>
    public class DomainTraceIdentifierMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor que inicializa una nueva instancia de <see cref="DomainTraceIdentifierMiddleware"/>.
        /// </summary>
        /// <param name="next">El siguiente delegado en la cadena de middleware.</param>
        public DomainTraceIdentifierMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Método invocado por el runtime de ASP.NET Core para procesar la solicitud HTTP.
        /// </summary>
        /// <param name="context">El contexto de la solicitud HTTP.</param>
        /// <param name="traceIdentifier">El identificador de seguimiento personalizado para la solicitud.</param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        public async Task InvokeAsync(HttpContext context, TraceIdentifier traceIdentifier)
        {
            // Asigna el identificador de seguimiento personalizado al contexto de la solicitud.
            context.TraceIdentifier = traceIdentifier.Id;

            // Continúa con el siguiente middleware en la cadena.
            await _next(context);
        }
    }
}