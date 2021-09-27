using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualBasic;
using WebStorageSystem.Models;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Data
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
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, DataTableRequest request)
        {
            int col = request.Order[0].Column;
            string name = request.Columns[col].Data;
            return request.Order[0].Dir == DataTableRequestOrderDirection.asc ? query.OrderBy(name) : query.OrderByDescending(name);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a property.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="query">A sequence of values to order.</param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        // Source: https://stackoverflow.com/questions/31955025/generate-ef-orderby-expression-by-string
        private static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            Type entityType = typeof(TSource);

            //Create x => x.PropName
            PropertyInfo propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            LambdaExpression selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            Type enumerableType = typeof(Queryable);
            MethodInfo method = enumerableType.GetMethods()
                .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
                .Where(m =>
                {
                    var parameters = m.GetParameters().ToList();
                    //Put more restriction here to ensure selecting the right overload                
                    return parameters.Count == 2;//overload that has 2 parameters
                }).Single();
            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderBy(selector), with query and selector: x => x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            IOrderedQueryable<TSource> newQuery = (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
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
        private static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            Type entityType = typeof(TSource);

            //Create x => x.PropName
            PropertyInfo propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            LambdaExpression selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderByDescending() method.
            Type enumerableType = typeof(Queryable);
            MethodInfo method = enumerableType.GetMethods()
                .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition)
                .Where(m =>
                {
                    var parameters = m.GetParameters().ToList();
                    //Put more restriction here to ensure selecting the right overload                
                    return parameters.Count == 2;//overload that has 2 parameters
                }).Single();
            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderBy(selector), with query and selector: x => x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            IOrderedQueryable<TSource> newQuery = (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

        /// <summary>
        /// 
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

                string colName = column.Data;
                string searchVal = column.Search.Value;

                if(searchVal == null) continue;

                ParameterExpression arg = Expression.Parameter(entityType, "x");


                MethodInfo method;
                ConstantExpression constant;
                if (DateTime.TryParse(searchVal, out DateTime dt)) //TODO: Check
                {
                    method = typeof(DateTime).GetMethod("Compare", new[] { typeof(DateTime) });
                    constant = Expression.Constant(dt, typeof(DateTime));
                }
                else if(bool.TryParse(searchVal, out bool b))
                {
                    method = typeof(bool).GetMethod("Equals", new[] { typeof(bool) });
                    constant = Expression.Constant(b, typeof(bool));
                }
                else
                {
                    method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    constant = Expression.Constant(searchVal, typeof(string));
                }

                MemberExpression property = Expression.Property(arg, colName);
                MethodCallExpression expression = Expression.Call(property, method, constant);
                Expression<Func<TSource, bool>> lambda = Expression.Lambda<Func<TSource, bool>>(expression, arg);
                q = q.Where(lambda);
            }

            return q;
        }
    }
}
