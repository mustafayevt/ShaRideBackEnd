using System;
using System.Collections.Generic;

namespace ShaRide.Application.Extensions
{
    public static class GenericExtensions
    {
        /// <summary>
        /// Simulates SQL's "IN" operator. Search a value in the passed arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host"></param>
        /// <param name="items">A list of items to search in.</param>
        /// <returns></returns>
        public static bool In<T>(this T host, params T[] items)
        {
            return Array.IndexOf(items, host) != -1;
        }
        
        /// <summary>
        /// Returns the set of items, made distinct by the selected value.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="selector">A function that selects a value to determine unique results.</param>
        /// <returns>IEnumerable&lt;TSource&gt;.</returns>
        public static IEnumerable<TSource> Distinct<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            HashSet<TResult> set = new HashSet<TResult>();

            foreach(var item in source)
            {
                var selectedValue = selector(item);

                if (set.Add(selectedValue))
                    yield return item;
            }
        }
    }
}