namespace Com.Coppel.Web.Api.Core.Application.Common
{
    /// <summary>
    /// Clase que representa una respuesta paginada.
    /// </summary>
    /// <typeparam name="T">Tipo de datos de los elementos en la respuesta.</typeparam>
    public class PaginatedResponse<T> where T : class
    {
        /// <summary>
        /// Obtiene o establece el número de página actual.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Obtiene o establece el límite de elementos por página.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Obtiene o establece el total de elementos en la respuesta.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Obtiene o establece los datos de la página actual.
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// Constructor por defecto de la clase PaginatedResponse.
        /// </summary>
        public PaginatedResponse()
        {
            Page = 0;
            Limit = 0;
            Total = 0;
            Data = new List<T>();
        }

        /// <summary>
        /// Constructor de la clase PaginatedResponse que recibe los parámetros de la respuesta paginada.
        /// </summary>
        /// <param name="page">Número de página actual.</param>
        /// <param name="limit">Límite de elementos por página.</param>
        /// <param name="total">Total de elementos en la respuesta.</param>
        /// <param name="data">Datos de la página actual.</param>
        public PaginatedResponse(int page, int limit, int total, List<T> data)
        {
            Page = page;
            Limit = limit;
            Total = total;
            Data = data;
        }
    }
}