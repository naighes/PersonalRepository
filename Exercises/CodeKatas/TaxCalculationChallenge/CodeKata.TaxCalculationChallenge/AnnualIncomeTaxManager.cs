using System;
using System.Collections.Generic;
using CodeKata.TaxCalculationChallenge.Extensions;

namespace CodeKata.TaxCalculationChallenge
{
    public class AnnualIncomeTaxManager<T> where T : struct, IComparable<T>
    {
        public AnnualIncomeTaxManager()
        {
            _policies = new HashSet<ITaxPolicy<T>>();
        }

        public T CalculateDuty(T income)
        {
            dynamic result = default(T);

            lock (_policies)
                _policies.ForEach(p => result += p.Apply(income));

            return result;
        }

        public void AddPolicy(ITaxPolicy<T> policy)
        {
            if (policy == null)
                throw new ArgumentNullException("policy");

            if (!_policies.Add(policy))
                throw new ArgumentException("A duplicated policy has been detected.", "policy");
        }

        private readonly ISet<ITaxPolicy<T>> _policies;
    }
}