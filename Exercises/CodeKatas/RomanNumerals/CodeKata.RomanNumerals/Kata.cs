using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace CodeKata.RomanNumerals
{
    public class Kata
    {
        [Theory]
        [InlineData(1, "I")]
        [InlineData(2, "II")]
        [InlineData(4, "IV")]
        [InlineData(5, "V")]
        [InlineData(6, "VI")]
        [InlineData(9, "IX")]
        [InlineData(10, "X")]
        [InlineData(11, "XI")]
        [InlineData(14, "XIV")]
        [InlineData(20, "XX")]
        [InlineData(24, "XXIV")]
        [InlineData(40, "XL")]
        [InlineData(49, "XLIX")]
        [InlineData(50, "L")]
        [InlineData(90, "XC")]
        [InlineData(99, "XCIX")]
        [InlineData(100, "C")]
        [InlineData(94, "XCIV")]
        [InlineData(101, "CI")]
        [InlineData(104, "CIV")]
        [InlineData(109, "CIX")]
        [InlineData(110, "CX")]
        [InlineData(399, "CCCXCIX")]
        [InlineData(349, "CCCXLIX")]
        [InlineData(500, "D")]
        [InlineData(400, "CD")]
        [InlineData(449, "CDXLIX")]
        [InlineData(450, "CDL")]
        [InlineData(499, "CDXCIX")]
        [InlineData(999, "CMXCIX")]
        [InlineData(2999, "MMCMXCIX")]
        [InlineData(3000, "MMM")]
        [InlineData(2944, "MMCMXLIV")]
        public void UseCases(Int32 number, String expected)
        {
            Assert.Equal(expected, Convert(number));
        }

        static readonly LinkedList<Tuple<Int32, Char[]>> Symbols = new LinkedList<Tuple<Int32, Char[]>>(new[]
                                                                                                    {
                                                                                                        new Tuple<Int32, Char[]>(1000, new[] {'M'}),
                                                                                                        new Tuple<Int32, Char[]>(900, new[] {'C', 'M'}),
                                                                                                        new Tuple<Int32, Char[]>(500, new[] {'D'}),
                                                                                                        new Tuple<Int32, Char[]>(400, new[] {'C', 'D'}),
                                                                                                        new Tuple<Int32, Char[]>(100, new[] {'C'}),
                                                                                                        new Tuple<Int32, Char[]>(90, new[] {'X', 'C'}),
                                                                                                        new Tuple<Int32, Char[]>(50, new[] {'L'}),
                                                                                                        new Tuple<Int32, Char[]>(40, new[] {'X', 'L'}),
                                                                                                        new Tuple<Int32, Char[]>(10, new[] {'X'}),
                                                                                                        new Tuple<Int32, Char[]>(9, new[] {'I', 'X'}),
                                                                                                        new Tuple<Int32, Char[]>(5, new[] {'V'}),
                                                                                                        new Tuple<Int32, Char[]>(4, new[] {'I', 'V'}),
                                                                                                        new Tuple<Int32, Char[]>(1, new[] {'I'}) 
                                                                                                    });

        static String Convert(Int32 input, StringBuilder source, LinkedListNode<Tuple<Int32, Char[]>> node)
        {
            if (node == null)
                return source.ToString();

            for (var i = 0; i < input.DivideBy(node.Value.Item1); i++)
                source.Append(node.Value.Item2);

            return Convert(input.ModuleBy(node.Value.Item1), source, node.Next);
        }

        static String Convert(Int32 input)
        {
            return Convert(input, new StringBuilder(), Symbols.First);
        }
    }

    public static class Int32Extensions
    {
        public static Int32 DivideBy(this Int32 source, Int32 divisor)
        {
            return source/divisor;
        }

        public static Int32 ModuleBy(this Int32 source, Int32 divisor)
        {
            return source % divisor;
        }
    }
}