using System;

namespace CodeKata.TaxCalculationChallenge
{
    public class RangeTaxPolicy<T> : ITaxPolicy<T> where T : struct, IComparable<T>
    {
        public RangeTaxPolicy(IRangeOverlapAlgorithm<T> rangeOverlapAlgorithm, T start, T end, T rate)
        {
            _rangeOverlapAlgorithm = rangeOverlapAlgorithm;
            _start = start;
            _end = end;
            _rate = rate;
        }

        private readonly IRangeOverlapAlgorithm<T> _rangeOverlapAlgorithm;
        private readonly T _start;
        private readonly T _end;
        private readonly T _rate;

        public T Apply(T value)
        {
            var target = (dynamic) _rangeOverlapAlgorithm.GetRangeOverlap(_start, _end, value);
            return target*_rate;
        }

        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (ReferenceEquals(null, obj))
                return false;

            var policy = obj as RangeTaxPolicy<T>;
            return policy != null && (_start.Equals(policy._start) &&
                                      _end.Equals(policy._end));
        }

        public override Int32 GetHashCode()
        {
            var hash = 88;
            hash = hash*13 + _start.GetHashCode();
            hash = hash*13 + _end.GetHashCode();
            return hash;
        }
    }
}