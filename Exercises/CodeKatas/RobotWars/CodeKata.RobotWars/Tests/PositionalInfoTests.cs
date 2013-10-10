using System;
using Xunit;

namespace CodeKata.RobotWars.Tests
{
    public class PositionalInfoTests
    {
        [Fact]
        public void BuildFromArray()
        {
            var info = Robot.New("1 2 N");
            Assert.Equal(1, info.Position.X);
            Assert.Equal(2, info.Position.Y);
            Assert.Equal('N', info.Orientation);
        }

        [Fact]
        public void EnsurePreconditions()
        {
            Assert.Throws<FormatException>(() => Robot.New("N 2 N"));
            Assert.Throws<FormatException>(() => Robot.New("1 N N"));
            Assert.Throws<FormatException>(() => Robot.New("1 2"));
            Assert.Throws<FormatException>(() => Robot.New("1 2 Z"));
        }
    }
}