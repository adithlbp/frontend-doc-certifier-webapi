namespace Com.Coppel.Web.Api.Presentation.Common
{
    /// <summary>
    /// Representa un identificador único para rastreo, utilizando un GUID.
    /// </summary>
    public class TraceIdentifier
    {
        /// <summary>
        /// Obtiene o establece el identificador único para el objeto TraceIdentifier.
        /// </summary>
        /// <value>
        /// El identificador único como una cadena.
        /// </value>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Devuelve una representación de cadena del identificador único.
        /// </summary>
        /// <returns>El identificador único como una cadena.</returns>
        public override string ToString() => Id;
    }
}