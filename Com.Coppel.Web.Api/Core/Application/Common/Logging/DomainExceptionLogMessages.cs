namespace Com.Coppel.Web.Api.Core.Application.Common.Logging
{
    /// <summary>
    /// LoggerMessage delegates para el manejo de excepciones de dominio
    /// EventIds en rango 9000-9999
    /// </summary>
    public static class DomainExceptionLogMessages
    {
        // Exception Middleware - Rango 9000-9099
        public static readonly Action<ILogger, string, FailureApiResponse, Exception?> ErrorEnProcesamiento =
            LoggerMessage.Define<string, FailureApiResponse>(LogLevel.Error, new EventId(9001, "ErrorEnProcesamiento"),
                "Error en {Path}: {Response}");
    }
}