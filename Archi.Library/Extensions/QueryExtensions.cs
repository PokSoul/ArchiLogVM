using Archi.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Archi.Library.Extensions
{
    public static class QueryExtensions
    {
        public static IOrderedQueryable<TModel> Sort<TModel>(this IQueryable<TModel> query, Params param)
        {
            var parameter = Expression.Parameter(typeof(TModel), "x");
            if (param.HasAscOrder())
            {
                string champ = param.Asc;
                //string champ2 = param.Desc;
                Console.WriteLine();
                //var property = typeof(TModel).GetProperty(champ, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                // return query.OrderBy(x => typeof(TModel).GetProperty(champ, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public));

                //créer lambda

                var property = Expression.Property(parameter, champ);
                var o = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<TModel, object>>(o, parameter);

                //utilise
                return query.OrderBy(lambda);

            }
            else if (param.HasDescOrder())
            {
                string champ = param.Desc;
                //string champ2 = param.Desc;
                Console.WriteLine();
                //var property = typeof(TModel).GetProperty(champ, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                // return query.OrderBy(x => typeof(TModel).GetProperty(champ, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public));

                //créer lambda

                var property = Expression.Property(parameter, champ);
                var o = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<TModel, object>>(o, parameter);

                //utilise
                return query.OrderByDescending(lambda);
            }
            else
                return (IOrderedQueryable<TModel>)query;
        }
        public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, Params param)
        {
            string search = param.Search;
            if (query == null) throw new ArgumentNullException(nameof(query));
            if (string.IsNullOrWhiteSpace(search)) return query;

            var parameter = Expression.Parameter(typeof(T), "e");
            // The following simulates closure to let EF Core create parameter rather than constant value (in case you use `Expresssion.Constant(search)`)
            var value = Expression.Property(Expression.Constant(new { search }), nameof(search));
            var body = SearchStrings(parameter, value);
            if (body == null) return query;

            var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);
            return query.Where(predicate);
        }

        static Expression SearchStrings(Expression target, Expression search)
        {
            Expression result = null;

            var properties = target.Type
              .GetProperties()
              .Where(x => x.CanRead);

            foreach (var prop in properties)
            {
                Expression condition = null;
                var propValue = Expression.MakeMemberAccess(target, prop);
                if (prop.PropertyType == typeof(string))
                {
                    var comparand = Expression.Call(propValue, nameof(string.ToLower), Type.EmptyTypes);
                    condition = Expression.Call(comparand, nameof(string.Contains), Type.EmptyTypes, search);
                }
                else if (!prop.PropertyType.Namespace.StartsWith("System."))
                {
                    condition = SearchStrings(propValue, search);
                }
                if (condition != null)
                    result = result == null ? condition : Expression.OrElse(result, condition);
            }

            return result;
        }
    }
}