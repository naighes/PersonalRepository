using System;
using System.Collections.Generic;
using Xunit;

namespace CodeKata.FizzBuzz
{
    public class GameTests
    {
        public GameTests()
        {
            _writer = new Printer();
            _game = new Game(_writer.Print);   
        }

        private readonly Printer _writer;
        private readonly Game _game;

        [Fact]
        public void TestDivisibleByTrhree()
        {
            _game.Run(3, 1);
            Assert.Equal("Fizz", _writer.Text);
        }

        [Fact]
        public void TestDivisibleByFive()
        {
            _game.Run(5, 1);
            Assert.Equal("Buzz", _writer.Text);
        }

        [Fact]
        public void TestFirstFiveNumbers()
        {
            _game.Run(1, 5);
            Assert.Equal("1 2 Fizz 4 Buzz", _writer.Text);
        }

        [Fact]
        public void TestFirstFifteenNumbers()
        {
            _game.Run(1, 15);
            Assert.Equal("1 2 Fizz 4 Buzz Fizz 7 8 Fizz Buzz 11 Fizz 13 14 FizzBuzz", _writer.Text);
        }

        class Printer
        {
            internal Printer()
            {
                _lines = new List<String>();
            }

            internal String Text
            {
                get { return String.Join(" ", _lines); }
            }

            private readonly IList<String> _lines;

            public void Print(String value)
            {
                _lines.Add(value);
            }
        }
    }
}