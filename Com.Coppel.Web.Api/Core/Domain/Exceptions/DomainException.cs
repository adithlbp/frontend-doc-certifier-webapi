namespace Com.Coppel.Web.Api.Core.Domain.Exceptions
{
    /// <summary>
    /// Clase base abstracta para excepciones en el dominio.
    /// </summary>
    public abstract class DomainException : Exception
    {
        /// <summary>
        /// Código de la excepción.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Mensaje de desarrollo adicional.
        /// </summary>
        public string? DevMessage { get; }

        /// <summary>
        /// Constructor de la clase DomainException.
        /// </summary>
        /// <param name="message">Mensaje de la excepción.</param>
        /// <param name="code">Código de la excepción.</param>
        /// <param name="devMessage">Mensaje de desarrollo adicional.</param>
        protected DomainException(string message, string code, string? devMessage = null) : base(message)
        {
            Code = code;
            DevMessage = devMessage;
        }

        /// <summary>
        /// Crea una excepción para un campo requerido.
        /// </summary>
        /// <param name="field">Nombre del campo requerido.</param>
        /// <returns>Excepción MissingFieldException.</returns>
        public static MissingFieldException RequiredField(string field) => new(field);

        /// <summary>
        /// Crea una excepción para un campo con longitud mínima.
        /// </summary>
        /// <param name="field">Nombre del campo.</param>
        /// <param name="minLength">Longitud mínima requerida.</param>
        /// <returns>Excepción FieldMinLengthException.</returns>
        public static FieldMinLengthException MinLength(string field, int minLength) => new(field, minLength);

        /// <summary>
        /// Crea una excepción para un campo con longitud máxima.
        /// </summary>
        /// <param name="field">Nombre del campo.</param>
        /// <param name="maxLength">Longitud máxima permitida.</param>
        /// <returns>Excepción FieldMaxLengthException.</returns>
        public static FieldMaxLengthException MaxLength(string field, int maxLength) => new(field, maxLength);

        /// <summary>
        /// Crea una excepción para una entidad no encontrada.
        /// </summary>
        /// <param name="message">Mensaje de la excepción.</param>
        /// <returns>Excepción RecordNotFoundException.</returns>
        public static RecordNotFoundException EntityNotFound(string message) => new(message);

        /// <summary>
        /// Crea una excepción para una solicitud incorrecta.
        /// </summary>
        /// <param name="message">Mensaje de la excepción.</param>
        /// <returns>Excepción BadRequestException.</returns>
        public static BadRequestException BadRequest(string message) => new(message);
    }
}