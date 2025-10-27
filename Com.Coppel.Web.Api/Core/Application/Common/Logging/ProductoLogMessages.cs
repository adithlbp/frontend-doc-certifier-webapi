namespace Com.Coppel.Web.Api.Core.Application.Common.Logging
{
    /// <summary>
    /// LoggerMessage delegates para el módulo de Productos
    /// EventIds en rango 1000-1999
    /// </summary>
    public static class ProductoLogMessages
    {
        // Controller - Rango 1000-1099
        public static readonly Action<ILogger, Guid, Exception?> ObteniendoProductoPorId =
            LoggerMessage.Define<Guid>(LogLevel.Information, new EventId(1001, "ObteniendoProductoPorId"),
                "Obteniendo producto con id {Id}");

        public static readonly Action<ILogger, Exception?> ObteniendoProductosPaginados =
            LoggerMessage.Define(LogLevel.Information, new EventId(1002, "ObteniendoProductosPaginados"),
                "Obteniendo productos paginados");

        public static readonly Action<ILogger, string, Exception?> CreandoNuevoProducto =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1003, "CreandoNuevoProducto"),
                "Creando nuevo producto: {Nombre}");

        // Query Handlers - Rango 1100-1199
        public static readonly Action<ILogger, Guid, Exception?> BuscandoProducto =
            LoggerMessage.Define<Guid>(LogLevel.Information, new EventId(1101, "BuscandoProducto"),
                "Obteniendo producto con ID: {Id}");

        public static readonly Action<ILogger, Guid, Exception?> ProductoNoEncontrado =
            LoggerMessage.Define<Guid>(LogLevel.Warning, new EventId(1102, "ProductoNoEncontrado"),
                "Producto no encontrado con ID: {Id}");

        public static readonly Action<ILogger, string, Exception?> ProductoEncontrado =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1103, "ProductoEncontrado"),
                "Producto encontrado: {Nombre}");

        public static readonly Action<ILogger, int, int, Exception?> BuscandoProductosPaginados =
            LoggerMessage.Define<int, int>(LogLevel.Information, new EventId(1104, "BuscandoProductosPaginados"),
                "Obteniendo productos paginados - Página: {Page}, Límite: {Limit}");

        public static readonly Action<ILogger, int, int, Exception?> ProductosObtenidos =
            LoggerMessage.Define<int, int>(LogLevel.Information, new EventId(1105, "ProductosObtenidos"),
                "Se obtuvieron {Count} productos de {Total} total");

        // Command Handlers - Rango 1200-1299
        public static readonly Action<ILogger, string, Exception?> ProcesandoCreacionProducto =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1201, "ProcesandoCreacionProducto"),
                "Creando nuevo producto: {Nombre}");

        public static readonly Action<ILogger, Guid, Exception?> ProductoCreadoExitosamente =
            LoggerMessage.Define<Guid>(LogLevel.Information, new EventId(1202, "ProductoCreadoExitosamente"),
                "Producto creado exitosamente con ID: {Id}");
    }
}