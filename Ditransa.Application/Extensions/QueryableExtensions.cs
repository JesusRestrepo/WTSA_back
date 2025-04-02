using Microsoft.EntityFrameworkCore;
using Ditransa.Shared;
using System.Linq.Expressions;
using System.Reflection;

namespace Ditransa.Application.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken) where T : class
        {
            try
            {
                pageNumber = pageNumber == 0 ? 1 : pageNumber;
                pageSize = pageSize == 0 ? 10 : pageSize;
                int count = await source.CountAsync();
                pageNumber = pageNumber <= 0 ? 1 : pageNumber;
                List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
                return PaginatedResult<T>.Create(items, count, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, bool ascending = true)
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type, "p");
            PropertyInfo property;
            Expression propertyAccess;
            if (ordering.Contains('.'))
            {
                // support to be sorted on child fields.
                String[] childProperties = ordering.Split('.');
                property = type.GetProperty(childProperties[0]);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
                for (int i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(childProperties[i]);
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = typeof(T).GetProperty(ordering);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable),
                                                             ascending ? "OrderBy" : "OrderByDescending",
                                                             new[] { type, property.PropertyType }, source.Expression,
                                                             Expression.Quote(orderByExp));
            //return  source.OrderBy(x => orderByExp);
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}