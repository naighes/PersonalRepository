using System;
using CodeKata.TaxCalculationChallenge.Extensions;

namespace CodeKata.TaxCalculationChallenge
{
    public class DefaultRangeOverlapAlgorithm<T> : IRangeOverlapAlgorithm<T> where T : struct, IComparable<T>
    {
        public T GetRangeOverlap(T start, T end, T value)
        {
            var a = value - (dynamic) start;
            var b = value - (dynamic) end;
            return ((T) a).IsLowerThanOrEqualTo(default(T))
                       ? default(T)
                       : a - (((T) b).IsLowerThan(default(T)) ? default(T) : b);
        }
    }
}