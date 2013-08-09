using System;
using Xunit;

namespace CodeKata.TaxCalculationChallenge.Tests
{
    public class DefaultRangeOverlapAlgorithmFixtures
    {
        [Fact]
        public void FindOverlap()
        {
            var instance = new DefaultRangeOverlapAlgorithm<Decimal>();

            RangeOverlapAlgorithmInputEvaluator<Decimal>.With(instance)
                                                        .Input(1000M)
                                                        .Between(998M, 1002M)
                                                        .Expect(2M);

            RangeOverlapAlgorithmInputEvaluator<Decimal>.With(instance)
                                                        .Input(1001M)
                                                        .Between(998M, 1002M)
                                                        .Expect(3M);

            RangeOverlapAlgorithmInputEvaluator<Decimal>.With(instance)
                                                        .Input(1004M)
                                                        .Between(998M, 1002M)
                                                        .Expect(4M);

            RangeOverlapAlgorithmInputEvaluator<Decimal>.With(instance)
                                                        .Input(1003M)
                                                        .Between(998M, 1002M)
                                                        .Expect(4M);

            RangeOverlapAlgorithmInputEvaluator<Decimal>.With(instance)
                                                        .Input(997M)
                                                        .Between(998M, 1002M)
                                                        .Expect(Decimal.Zero);

            RangeOverlapAlgorithmInputEvaluator<Decimal>.With(instance)
                                                        .Input(998M)
                                                        .Between(998M, 1002M)
                                                        .Expect(Decimal.Zero);

            RangeOverlapAlgorithmInputEvaluator<Decimal>.With(instance)
                                                        .Input(1231.9M)
                                                        .Between(998M, 1002M)
                                                        .Expect(4M);

            RangeOverlapAlgorithmInputEvaluator<Decimal>.With(instance)
                                                        .Input(1.243M)
                                                        .Between(1.233M, 1.345M)
                                                        .Expect(0.01M);
        }
    }

    public class RangeOverlapAlgorithmInputEvaluator<T> where T : struct, IComparable<T>
    {
        public static RangeOverlapAlgorithmInputEvaluator<T> With(IRangeOverlapAlgorithm<T> algorithm)
        {
            return new RangeOverlapAlgorithmInputEvaluator<T>(algorithm);
        }

        private readonly IRangeOverlapAlgorithm<T> _algorithm;

        private T _input;
        private T _start;
        private T _end;

        private RangeOverlapAlgorithmInputEvaluator(IRangeOverlapAlgorithm<T> algorithm)
        {
            _algorithm = algorithm;
        }

        public RangeOverlapAlgorithmInputEvaluator<T> Input(T input)
        {
            _input = input;
            return this;
        }

        public RangeOverlapAlgorithmInputEvaluator<T> Between(T start, T end)
        {
            _start = start;
            _end = end;
            return this;
        }

        public void Expect(T expected)
        {
            Assert.Equal(expected, _algorithm.GetRangeOverlap(_start, _end, _input));
        }
    }
}