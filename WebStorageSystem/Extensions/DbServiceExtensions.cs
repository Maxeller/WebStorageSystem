using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Models.DataTables;
using Z.EntityFramework.Plus;

namespace WebStorageSystem.Extensions
{
    public static class DbServiceExtensions
    {
        /// <summary>
        /// Sorts the elements of a sequence in order based on DataTables request.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="query">A sequence of values to order.</param>
        /// <param name="request">DataTables request</param>
        /// <returns>Ordered query based on DataTableRequest</returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query,
            DataTableRequest request)
        {
            if (request.Order == null) // Datatables has possibility of unordered 
            {
                return (IOrderedQueryable<TSource>)query;
            }

            int columnNumber = request.Order[0].Column;
            string columnName = request.Columns[columnNumber].Data;
            if (columnName == null) columnName = request.Columns[columnNumber + 1].Data; // Crutch for Transfer table
            return request.Order[0].Dir == DataTableRequestOrderDirection.asc
                ? query.OrderBy(columnName)
                : query.OrderByDescending(columnName);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a property.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="query">A sequence of values to order.</param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        // Source: https://stackoverflow.com/questions/31955025/generate-ef-orderby-expression-by-string
        // Source for nesting: https://www.codemag.com/Article/1607041/Simplest-Thing-Possible-Dynamic-Lambda-Expressions
        private static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            Type entityType = typeof(TSource);

            //Create x => x.PropName OR x => x.ParentClass.PropName
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = GetProperty(arg, propertyName);
            LambdaExpression selector = Expression.Lambda(property, arg);

            //Get System.Linq.Queryable.OrderBy() method.
            Type enumerableType = typeof(Queryable);
            MethodInfo method = enumerableType.GetMethods()
                .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
                .Where(m =>
                {
                    var parameters = m.GetParameters().ToList();
                    //Put more restriction here to ensure selecting the right overload                
                    return parameters.Count == 2; //overload that has 2 parameters
                }).Single();
            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method.MakeGenericMethod(entityType, property.Type);

            /*Call query.OrderBy(selector), with query and selector: x => x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            IOrderedQueryable<TSource> newQuery =
                (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

        // TODO: Merge with OrderBy() ?
        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a property.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="query">A sequence of values to order.</param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> query,
            string propertyName)
        {
            Type entityType = typeof(TSource);

            //Create x => x.PropName OR x => x.ParentClass.PropName
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = GetProperty(arg, propertyName);
            LambdaExpression selector = Expression.Lambda(property, arg);

            //Get System.Linq.Queryable.OrderByDescending() method.
            Type enumerableType = typeof(Queryable);
            MethodInfo method = enumerableType.GetMethods()
                .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition)
                .Where(m =>
                {
                    var parameters = m.GetParameters().ToList();
                    //Put more restriction here to ensure selecting the right overload                
                    return parameters.Count == 2; //overload that has 2 parameters
                }).Single();
            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method.MakeGenericMethod(entityType, property.Type);

            /*Call query.OrderBy(selector), with query and selector: x => x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            IOrderedQueryable<TSource> newQuery =
                (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

        /// <summary>
        /// Searches elements based on DataTables request.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="query">A sequence of values to search</param>
        /// <param name="request">DataTables request</param>
        /// <returns></returns>
        // Based on: https://github.com/dotnet/efcore/issues/20057
        public static IQueryable<TSource> Search<TSource>(this IQueryable<TSource> query, DataTableRequest request)
        {
            Type entityType = typeof(TSource);

            IQueryable<TSource> q = query;

            foreach (var column in request.Columns)
            {
                if (!column.Searchable) continue;

                string columnName = column.Data;
                string searchValue = column.Search.Value;

                if (searchValue == null) continue;
                if (columnName.Contains("BundledUnits")) continue;

                ParameterExpression arg = Expression.Parameter(entityType, "x");

                MethodInfo method;
                ConstantExpression constant;
                if (DateTime.TryParse(searchValue, out DateTime dt) && columnName.Contains("Date"))
                {
                    method = typeof(DateTime).GetMethod("CompareTo", new[] { typeof(DateTime) });
                    DateTime dtUtc = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                    constant = Expression.Constant(dtUtc, typeof(DateTime));
                }
                else if (bool.TryParse(searchValue, out bool b))
                {
                    method = typeof(bool).GetMethod("Equals", new[] { typeof(bool) });
                    constant = Expression.Constant(b, typeof(bool));
                }
                else
                {
                    method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    constant = Expression.Constant(searchValue, typeof(string));
                }

                MemberExpression property = GetProperty(arg, columnName);
                MethodCallExpression expression = Expression.Call(property, method, constant);

                Expression<Func<TSource, bool>> lambda;
                if (columnName.Contains("Date"))
                {
                    ConstantExpression zero = Expression.Constant(0, typeof(int));
                    BinaryExpression
                        binaryExpression =
                            Expression.GreaterThanOrEqual(expression,
                                zero); // TODO: Check CompareTo docs for diff options
                    lambda = Expression.Lambda<Func<TSource, bool>>(binaryExpression, arg);
                }
                else
                {
                    lambda = Expression.Lambda<Func<TSource, bool>>(expression, arg);
                }

                q = q.Where(lambda);
            }

            return q;
        }

        private static MemberExpression GetProperty(Expression arg, string propertyName)
        {
            int index = propertyName.IndexOf('.');
            while (index != -1)
            {
                arg = Expression.Property(arg, propertyName.Substring(0, index));
                propertyName = propertyName.Substring(index + 1);
                index = propertyName.IndexOf('.');
            }

            MemberExpression property = Expression.Property(arg, propertyName);
            return property;
        }

        public static async Task<IEnumerable<TSource>> SpecialitySearch<TSource>(this IQueryable<TSource> query, DataTableRequest request)
        {
            foreach (var column in request.Columns)
            {
                if (!column.Searchable) continue;

                string columnName = column.Data;
                string searchValue = column.Search.Value;

                if (searchValue == null) continue;
                
                switch (columnName)
                {
                    case "BundledUnits":
                    {
                        return await ((IQueryable<Bundle>)query)
                            .IncludeFilter(b => b.BundledUnits.Where(u => u.InventoryNumber.Contains(searchValue)))
                            .ToListAsync() as IEnumerable<TSource>;
                    }

                    default: continue;
                }
            }

            return await query.ToListAsync();
        }
    }
}