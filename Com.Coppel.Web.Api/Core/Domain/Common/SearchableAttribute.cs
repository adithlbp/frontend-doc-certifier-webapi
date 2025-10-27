namespace Com.Coppel.Web.Api.Core.Domain.Common
{
    /// <summary>
    /// Atributo personalizado que indica que una propiedad es buscable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SearchableAttribute : Attribute { }
}