using Archi.Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
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
                Console.WriteLine();
   
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
                Console.WriteLine();
  
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
        public static IQueryable<TModel> ApplySearch<TModel>(this IQueryable<TModel> query, Params param, IQueryCollection search)
        {
            var champ = new Dictionary<string, StringValues>();
        
            foreach (KeyValuePair<string, StringValues> item in search)
            {
                var property = typeof(Params).GetProperty(item.Key, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                if (property != null)
                {
                    champ.Add(item.Key, item.Value);
                }
            
            }

            var result = query;
            var parameter = Expression.Parameter(typeof(TModel), "x");
            var res = champ.Select(e => LikeExpression<TModel>(parameter, e.Key, e.Value ));
            BinaryExpression bin = null;
            foreach (var data in res) {

                //var value = data.Value;
         
                //var property = Expression.Property(parameter,data.Key);
                //Expression constant;
                //constant = Expression.Constant(data);
           

                //var expComp = Expression.Equal(property,constant);
                //if (bin == null) 
                //{
                //    bin = expComp;
                //}
                //else
                //{
                //    bin = Expression.And(bin, expComp);
                //}
                var lambda = Expression.Lambda<Func<TModel, bool>>(bin, parameter);
                result = result.Where(lambda);

            }


            return result;
        }

        public static Expression LikeExpression<TModel>(ParameterExpression param, string key, string value)
        {

            var propertyInfo = typeof(TModel).GetProperty(key, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var member = Expression.Property(param, propertyInfo);

            //var startwith = value.StartsWith("*");
            //var endswith = value.StartsWith("*");
            //var searchvalue = value.Replace("*", "");

            Expression constant;
            Expression convert;
            Expression exp;

            constant = Expression.Constant(value);
            convert = Expression.Convert(member, typeof(string));


            //    if (endsWith && startWith)
            //    {
            //        exp = Expression.Call(convert, "Contains", null, constant);
            //    }
            //    else if (startWith)
            //    {
            //        exp = Expression.Call(convert, "EndsWith", null, constant);
            //    }
            //    else if (endsWith)
            //        exp = Expression.Call(convert, "StartsWith", null, constant);
            //    else
            //    {
                    exp = Expression.Equal(convert, constant);
            //    }
            return exp;

        }
    }
}