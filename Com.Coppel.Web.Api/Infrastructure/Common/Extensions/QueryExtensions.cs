using System.Linq.Expressions;
using System.Text;

using Com.Coppel.Web.Api.Core.Application.Common;
using Com.Coppel.Web.Api.Core.Domain.Common;
using Com.Coppel.Web.Api.Core.Domain.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace Com.Coppel.Web.Api.Infrastructure.Common.Extensions
{
    /// <summary>
    /// Extensiones para consultas IQueryable
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        /// Aplica los parámetros de consulta a una consulta IQueryable.
        /// </summary>
        public static IQueryable<T> ApplyQueryParams<T>(this IQueryable<T> query, QueryParams queryParams) where T : class
        {
            if (queryParams == null) return query;

            query = query
                .WithSearch(queryParams.Search)
                .WithSorting(queryParams.Sort);

            if (queryParams is PaginatedQueryParams paginatedQueryParams)
                return query.WithPagination(paginatedQueryParams.Page, paginatedQueryParams.Limit);

            return query;
        }

        private static IQueryable<T> WithPagination<T>(this IQueryable<T> query, int? page, int? limit) where T : class
        {
            if (page == null || limit == null) return query;
            if (page.Value < 1) page = 1;
            if (limit.Value < 1) limit = 10;
            var skip = (page.Value - 1) * limit.Value;
            return query.Skip(skip).Take(limit.Value);
        }

        private static IQueryable<T> WithSorting<T>(this IQueryable<T> query, string? sort) where T : class
        {
            if (string.IsNullOrEmpty(sort)) return query;
            var sortArray = sort.Split(',');
            foreach (var item in sortArray)
            {
                var parts = item.Split(' ');
                var property = parts[0].Trim().ToPascalCase();
                var direction = parts.Length > 1 ? parts[1] : "asc";
                query = query.OrderByProperty(property, direction);
            }
            return query;
        }

        private static IQueryable<T> WithSearch<T>(this IQueryable<T> query, string? search) where T : class
        {
            if (string.IsNullOrEmpty(search)) return query;
            search = SanitizeSearch(search);
            var properties = typeof(T).GetProperties().Where(x => x.GetCustomAttributes(typeof(SearchableAttribute), false).Length > 0);
            var efLikeMethod = typeof(DbFunctionsExtensions).GetMethod("Like", new[] { typeof(DbFunctions), typeof(string), typeof(string) }) ?? throw new MethodNotFoundException("No se encontró el método Like de EF");
            var parameter = Expression.Parameter(typeof(T), "x");
            var body = properties
                .Select(property =>
                {
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var likeMethod = Expression.Call(efLikeMethod, Expression.Constant(EF.Functions), propertyAccess, Expression.Constant($"%{search}%"));
                    return likeMethod;
                })
                .Aggregate<Expression, Expression>(Expression.Constant(false), Expression.OrElse);
            return query.Where(Expression.Lambda<Func<T, bool>>(body, parameter));
        }

        private static string SanitizeSearch(string search)
        {
            var sb = new StringBuilder();
            foreach (var c in search)
            {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> query, string property, string direction) where T : class
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");
            var propertyInfo = type.GetProperty(property);
            if (propertyInfo == null) return query;

            var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            var methodName = direction == "desc" ? "OrderByDescending" : "OrderBy";
            var resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, propertyInfo.PropertyType }, query.Expression, Expression.Quote(orderByExp));

            return query.Provider.CreateQuery<T>(resultExp);
        }

        /// <summary>
        /// Convierte una cadena en formato CamelCase a PascalCase.
        /// </summary>
        public static string ToPascalCase(this string str)
        {
            if (string.IsNullOrEmpty(str) || !char.IsLower(str[0])) return str;
            var chars = str.ToCharArray();
            chars[0] = char.ToUpperInvariant(chars[0]);
            return new string(chars);
        }
    }
}