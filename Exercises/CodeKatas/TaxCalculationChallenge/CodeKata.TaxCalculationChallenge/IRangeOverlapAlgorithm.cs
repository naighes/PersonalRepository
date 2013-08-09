using System;

namespace CodeKata.TaxCalculationChallenge
{
    public interface IRangeOverlapAlgorithm<T> where T : struct, IComparable<T>
    {
        T GetRangeOverlap(T start, T end, T value);
    }
}