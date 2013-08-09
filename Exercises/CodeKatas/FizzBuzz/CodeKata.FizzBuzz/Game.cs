using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeKata.FizzBuzz
{
    public class Game
    {
        public Game(Action<String> printer)
        {
            if (printer == null)
                throw new ArgumentNullException("printer");

            _rules = new Dictionary<Func<Int32, Boolean>, Action<Int32>>
            {
                {i => i.IsDivisibleBy(3) && i.IsDivisibleBy(5), i => printer("FizzBuzz")},
                {i => i.IsDivisibleBy(3), i => printer("Fizz")},
                {i => i.IsDivisibleBy(5), i => printer("Buzz")},
                {i => true, i => printer(i.ToString())}
            };
        }

        public void Run(Int32 start, Int32 count)
        {
            Enumerable.Range(start, count).ForEach(Process);
        }

        private void Process(Int32 input)
        {
            _rules.FirstOrDefault(r => r.Key(input)).Value(input);
        }

        private readonly IDictionary<Func<Int32, Boolean>, Action<Int32>> _rules;
    }

    public static class Int32Extensions
    {
        public static Boolean IsDivisibleBy(this Int32 dividend, Int32 divisor)
        {
            return dividend % divisor == 0;
        }
    }

    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }
    }
}