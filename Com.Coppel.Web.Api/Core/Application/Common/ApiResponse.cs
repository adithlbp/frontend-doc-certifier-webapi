using System.Net;

namespace Com.Coppel.Web.Api.Core.Application.Common
{
    /// <summary>
    /// Clase base abstracta para todas las respuestas de API.
    /// </summary>
    public abstract class ApiResponse
    {
        /// <summary>
        /// Metadatos de la respuesta de API.
        /// </summary>
        public ApiResponseMetadata Meta { get; set; }

        /// <summary>
        /// Constructor de la clase ApiResponse.
        /// </summary>
        /// <param name="meta">Metadatos de la respuesta de API.</param>
        protected ApiResponse(ApiResponseMetadata meta)
        {
            Meta = meta;
        }

        /// <summary>
        /// Devuelve una representación en cadena de la respuesta de API.
        /// </summary>
        /// <returns>Representación en cadena de la respuesta de API.</returns>
        public override string ToString() => $"ApiResponse: {{ Meta: {Meta} }}";

        /// <summary>
        /// Crea una instancia de SuccessApiResponse con el estado "Ok".
        /// </summary>
        /// <typeparam name="T">Tipo de datos de la respuesta.</typeparam>
        /// <param name="data">Datos de la respuesta.</param>
        /// <param name="transactionID">ID de transacción.</param>
        /// <returns>Instancia de SuccessApiResponse.</returns>
        public static SuccessApiResponse<T> Ok<T>(T? data, string transactionID) => new SuccessApiResponse<T>(data, transactionID);

        /// <summary>
        /// Crea una instancia de NoContentApiResponse con el estado "Ok" y sin datos.
        /// </summary>
        /// <param name="transactionID">ID de transacción.</param>
        /// <returns>Instancia de NoContentApiResponse.</returns>
        public static NoContentApiResponse Empty(string transactionID) => new NoContentApiResponse(ApiResponseMetadata.Ok(transactionID));

        /// <summary>
        /// Crea una instancia de FailureApiResponse con el estado "Error".
        /// </summary>
        /// <param name="transactionID">ID de transacción.</param>
        /// <param name="statusCode">Código de estado HTTP.</param>
        /// <param name="message">Mensaje de error.</param>
        /// <param name="devMessage">Mensaje de error para desarrolladores (opcional).</param>
        /// <returns>Instancia de FailureApiResponse.</returns>
        public static FailureApiResponse Error(string transactionID, HttpStatusCode statusCode, string message, string? devMessage = null) => new FailureApiResponse(transactionID, statusCode, message, devMessage);

        /// <summary>
        /// Crea una instancia de ResourceNotFoundApiResponse con el estado "NotFound".
        /// </summary>
        /// <param name="transactionID">ID de transacción.</param>
        /// <param name="message">Mensaje de error.</param>
        /// <param name="devMessage">Mensaje de error para desarrolladores (opcional).</param>
        /// <returns>Instancia de ResourceNotFoundApiResponse.</returns>
        public static ResourceNotFoundApiResponse NotFound(string transactionID, string message, string? devMessage = null) => new ResourceNotFoundApiResponse(transactionID, message, devMessage);
    }

    /// <summary>
    /// Clase que representa una respuesta exitosa de API con datos.
    /// </summary>
    /// <typeparam name="T">Tipo de datos de la respuesta.</typeparam>
    public class SuccessApiResponse<T> : ApiResponse
    {
        /// <summary>
        /// Datos de la respuesta.
        /// </summary>
        public T? Data { get; }

        /// <summary>
        /// Constructor de la clase SuccessApiResponse.
        /// </summary>
        /// <param name="data">Datos de la respuesta.</param>
        /// <param name="transactionID">ID de transacción.</param>
        public SuccessApiResponse(T? data, string transactionID) : base(ApiResponseMetadata.Ok(transactionID))
        {
            Data = data;
        }
    }

    /// <summary>
    /// Clase que representa una respuesta de API sin contenido.
    /// </summary>
    public class NoContentApiResponse : ApiResponse
    {
        /// <summary>
        /// Constructor de la clase NoContentApiResponse.
        /// </summary>
        /// <param name="meta">Metadatos de la respuesta de API.</param>
        public NoContentApiResponse(ApiResponseMetadata meta) : base(meta) { }
    }

    /// <summary>
    /// Clase que representa una respuesta de API con error.
    /// </summary>
    public class FailureApiResponse : ApiResponse
    {
        /// <summary>
        /// Constructor de la clase FailureApiResponse.
        /// </summary>
        /// <param name="transactionID">ID de transacción.</param>
        /// <param name="statusCode">Código de estado HTTP.</param>
        /// <param name="message">Mensaje de error.</param>
        /// <param name="devMessage">Mensaje de error para desarrolladores (opcional).</param>
        public FailureApiResponse(string transactionID, HttpStatusCode statusCode, string message, string? devMessage = null)
            : base(ApiResponseMetadata.Error(transactionID, statusCode, message, devMessage)) { }
    }

    /// <summary>
    /// Clase que representa una respuesta de API con recurso no encontrado.
    /// </summary>
    public class ResourceNotFoundApiResponse : ApiResponse
    {
        /// <summary>
        /// Constructor de la clase ResourceNotFoundApiResponse.
        /// </summary>
        /// <param name="transactionID">ID de transacción.</param>
        /// <param name="message">Mensaje de error.</param>
        /// <param name="devMessage">Mensaje de error para desarrolladores (opcional).</param>
        public ResourceNotFoundApiResponse(string transactionID, string message, string? devMessage = null)
            : base(ApiResponseMetadata.NotFound(transactionID, message, devMessage)) { }
    }

    /// <summary>
    /// Clase que representa los metadatos asociados con una respuesta de la API.
    /// </summary>
    public class ApiResponseMetadata
    {
        // Constantes que definen los posibles estados de la respuesta de la API.
        public static readonly string StatusOk = "OK";
        public static readonly string StatusError = "ERROR";
        public static readonly string StatusNotFound = "NOT_FOUND";

        /// <summary>
        /// El identificador único de la transacción.
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// El estado de la respuesta de la API.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// El código de estado HTTP asociado con la respuesta.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// El mensaje asociado con la respuesta de la API.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Marca de tiempo de la respuesta.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Mensaje adicional para el desarrollador.
        /// </summary>
        public string? DevMessage { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ApiResponseMetadata"/>.
        /// </summary>
        /// <param name="transactionID">El identificador de la transacción.</param>
        /// <param name="status">El estado de la respuesta.</param>
        /// <param name="statusCode">El código de estado HTTP de la respuesta.</param>
        /// <param name="message">El mensaje descriptivo de la respuesta.</param>
        /// <param name="devMessage">El mensaje adicional para el desarrollador (opcional).</param>
        public ApiResponseMetadata(string transactionID, string status, HttpStatusCode statusCode, string message, string? devMessage = null)
        {
            TransactionID = transactionID;
            Status = status;
            StatusCode = statusCode;
            Message = message;
            DevMessage = devMessage;
        }

        /// <summary>
        /// Convierte los metadatos de la respuesta a una representación de cadena.
        /// </summary>
        /// <returns>Una cadena que representa los metadatos de la respuesta de la API.</returns>
        public override string ToString() => $"ApiResponseMetadata: {{ TransactionID: {TransactionID}, Status: {Status}, StatusCode: {StatusCode}, Message: {Message}, Timestamp: {Timestamp} }}";

        /// <summary>
        /// Crea metadatos de respuesta para una operación exitosa.
        /// </summary>
        /// <param name="transactionID">El identificador de la transacción.</param>
        /// <param name="message">El mensaje descriptivo de la respuesta (opcional, por defecto "Success").</param>
        /// <param name="status">El estado de la respuesta (opcional, por defecto "OK").</param>
        /// <returns>Metadatos de respuesta para una operación exitosa.</returns>
        public static ApiResponseMetadata Ok(string transactionID, string message = "Success", string status = "OK") => new ApiResponseMetadata(transactionID, status, HttpStatusCode.OK, message);

        /// <summary>
        /// Crea metadatos de respuesta para una operación fallida.
        /// </summary>
        /// <param name="transactionID">El identificador de la transacción.</param>
        /// <param name="statusCode">El código de estado HTTP de la respuesta.</param>
        /// <param name="message">El mensaje de error para el usuario.</param>
        /// <param name="devMessage">El mensaje de error para el desarrollador (opcional).</param>
        /// <param name="status">El estado de la respuesta (opcional, por defecto "ERROR").</param>
        /// <returns>Metadatos de respuesta para una operación fallida.</returns>
        public static ApiResponseMetadata Error(string transactionID, HttpStatusCode statusCode, string message, string? devMessage = null, string status = "ERROR") => new ApiResponseMetadata(transactionID, status, statusCode, message, devMessage);

        /// <summary>
        /// Crea metadatos de respuesta cuando el recurso solicitado no se encuentra.
        /// </summary>
        /// <param name="transactionID">El identificador de la transacción.</param>
        /// <param name="message">El mensaje de error para el usuario.</param>
        /// <param name="devMessage">El mensaje de error para el desarrollador (opcional).</param>
        /// <returns>Metadatos de respuesta para un recurso no encontrado.</returns>
        public static ApiResponseMetadata NotFound(string transactionID, string message, string? devMessage = null) => new ApiResponseMetadata(transactionID, StatusNotFound, HttpStatusCode.NotFound, message, devMessage);
    }
}