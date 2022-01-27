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
                Console.WriteLine(champ);
                //var property = typeof(TModel).GetProperty(champ, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                // return query.OrderBy(x => typeof(TModel).GetProperty(champ, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public));

                //créer lambda
                
                var property = Expression.Property(parameter, champ);
                var o = Expression.Convert(property, typeof(object));
                var lambda = Expression.Lambda<Func<TModel, object>>(o, parameter);

                //utilise
                return query.OrderBy(lambda);

            }
            else if(param.HasDescOrder())
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
            else if(param.HasRange())
            {
                string Range = param.Range;
                string[] RangeSplit = Range.Split('-');
                int RangeValue = int.Parse(RangeSplit[1]) - int.Parse(RangeSplit[0]) + 1;
                int SkipValue = int.Parse(RangeSplit[0]);
                int MaxRange = query.Count();

                return (IOrderedQueryable<TModel>)query.Skip(SkipValue).Take(RangeValue);
            }
            



            else
                return (IOrderedQueryable<TModel>)query;
        }


    }
}