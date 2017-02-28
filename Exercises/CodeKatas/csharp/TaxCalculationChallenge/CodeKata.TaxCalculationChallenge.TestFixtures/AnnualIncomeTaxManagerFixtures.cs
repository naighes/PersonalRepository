using System;
using Rhino.Mocks;
using Xunit;

namespace CodeKata.TaxCalculationChallenge.Tests
{
    public class AnnualIncomeTaxManagerFixtures
    {
        [Fact]
        public void Calculation()
        {
            var instance = new AnnualIncomeTaxManager<Decimal>();
            var algorithm = new DefaultRangeOverlapAlgorithm<Decimal>();

            instance.AddPolicy(new RangeTaxPolicy<Decimal>(algorithm, Decimal.Zero, 5070M, 0.1M));
            instance.AddPolicy(new RangeTaxPolicy<Decimal>(algorithm, 5070M, 8660M, 0.14M));
            instance.AddPolicy(new RangeTaxPolicy<Decimal>(algorithm, 8660M, 14070M, 0.23M));
            instance.AddPolicy(new RangeTaxPolicy<Decimal>(algorithm, 14070M, 21240M, 0.3M));
            instance.AddPolicy(new RangeTaxPolicy<Decimal>(algorithm, 21240M, 40230M, 0.33M));
            instance.AddPolicy(new RangeTaxPolicy<Decimal>(algorithm, 40230M, Decimal.MaxValue, 0.45M));

            Assert.Equal(500M, instance.CalculateDuty(5000M));
            Assert.Equal(609.2M, instance.CalculateDuty(5800M));
            Assert.Equal(1087.8M, instance.CalculateDuty(9000M));
            Assert.Equal(2532.9M, instance.CalculateDuty(15000M));
            Assert.Equal(15068.1M, instance.CalculateDuty(50000M));
        }

        [Fact]
        public void DuplicatePolicyAdded()
        {
            var instance = new AnnualIncomeTaxManager<Decimal>();
            var algorithm = new MockRepository().Stub<IRangeOverlapAlgorithm<Decimal>>();
            var policy1 = new RangeTaxPolicy<Decimal>(algorithm, Decimal.Zero, 5070M, 0.1M);
            instance.AddPolicy(policy1);
            Assert.Throws<ArgumentException>(() => instance.AddPolicy(policy1));
        }

        [Fact]
        public void NullPolicyAdded()
        {
            var instance = new AnnualIncomeTaxManager<Decimal>();
            Assert.Throws<ArgumentNullException>(() => instance.AddPolicy(null));
        }
    }
}