using System;
using System.Collections.Generic;

namespace CodeKata.TaxCalculationChallenge.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var enumerator = source.GetEnumerator();

            while (enumerator.MoveNext())
                action(enumerator.Current);
        }
    }
}