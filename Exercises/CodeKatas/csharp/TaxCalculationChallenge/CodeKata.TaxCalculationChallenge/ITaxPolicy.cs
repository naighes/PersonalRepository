using System;

namespace CodeKata.TaxCalculationChallenge
{
    public interface ITaxPolicy<T> where T : struct, IComparable<T>
    {
        T Apply(T value);
    }
}