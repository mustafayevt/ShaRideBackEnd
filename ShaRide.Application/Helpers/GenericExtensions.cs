using System;

namespace ShaRide.Application.Helpers
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
    }
}