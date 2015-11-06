namespace Physicist.Types.Util
{
    using System;
    using System.Collections.Generic;

    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action) where T : class
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (var item in collection)
            {
                action.Invoke(item);
            }
        }
    }
}
