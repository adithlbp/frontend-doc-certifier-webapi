namespace Com.Coppel.Web.Api.Core.Application.Common
{
    /// <summary>
    /// Clase que representa los parámetros de consulta.
    /// </summary>
    public class QueryParams
    {
        /// <summary>
        /// Obtiene o establece el término de búsqueda.
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Obtiene o establece el criterio de ordenamiento.
        /// </summary>
        public string? Sort { get; set; }

        /// <summary>
        /// Obtiene o establece los elementos a incluir en la consulta.
        /// </summary>
        public string[]? Includes { get; set; }

        /// <summary>
        /// Devuelve una representación en cadena de los parámetros de consulta.
        /// </summary>
        /// <returns>Una cadena que representa los parámetros de consulta.</returns>
        public override string ToString() => $"QueryParams {{ Search: {Search}, Sort: {Sort}, Includes: {(Includes != null ? string.Join(",", Includes) : null)} }}";
    }

    /// <summary>
    /// Clase que representa los parámetros de consulta paginados.
    /// </summary>
    public class PaginatedQueryParams : QueryParams
    {
        /// <summary>
        /// Obtiene o establece el número de página.
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Obtiene o establece el límite de elementos por página.
        /// </summary>
        public int Limit { get; set; } = 10;

        /// <summary>
        /// Obtiene el desplazamiento a partir del cual se deben obtener los elementos.
        /// </summary>
        public int Offset => (Page - 1) * Limit;

        /// <summary>
        /// Devuelve una representación en cadena de los parámetros de consulta paginados.
        /// </summary>
        /// <returns>Una cadena que representa los parámetros de consulta paginados.</returns>
        public override string ToString() => $"PaginatedQueryParams {{ Page: {Page}, Limit: {Limit}, Offset: {Offset}, Search: {Search}, Sort: {Sort}, Includes: {(Includes != null ? string.Join(",", Includes) : null)} }}";
    }
}