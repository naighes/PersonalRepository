using System;
using Rhino.Mocks;
using Xunit;

namespace CodeKata.TaxCalculationChallenge.Tests
{
    public class RangeTaxPolicyFixtures
    {
        [Fact]
        public void RangeTaxPolicy_Appliance_Tests()
        {
            var instance = new RangeTaxPolicy<Decimal>(new DefaultRangeOverlapAlgorithm<Decimal>(),
                                                       Decimal.Zero,
                                                       5070M,
                                                       0.1M);
            Assert.Equal(500M, instance.Apply(5000M));
        }

        [Fact]
        public void RangeTaxPolicy_Equality_Tests()
        {
            var fakeAlgorithm = new MockRepository().Stub<IRangeOverlapAlgorithm<Decimal>>();

            var policy1 = new RangeTaxPolicy<Decimal>(fakeAlgorithm, Decimal.Zero, 5070M, 0.1M);
            Assert.Equal(policy1, policy1);
            Assert.Equal(policy1.GetHashCode(), policy1.GetHashCode());

            Assert.NotNull(policy1);

            var policy2 = new RangeTaxPolicy<Decimal>(fakeAlgorithm, Decimal.Zero, 5070M, 0.1M);
            Assert.Equal(policy1, policy2);
            Assert.Equal(policy2, policy1);
            Assert.Equal(policy1.GetHashCode(), policy2.GetHashCode());

            var policy3 = new RangeTaxPolicy<Decimal>(fakeAlgorithm, Decimal.Zero, 5071M, 0.1M);
            Assert.NotEqual(policy1, policy3);
            Assert.NotEqual(policy1.GetHashCode(), policy3.GetHashCode());
            Assert.NotEqual(policy2, policy3);
            Assert.NotEqual(policy2.GetHashCode(), policy3.GetHashCode());
        }
    }
}