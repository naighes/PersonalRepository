using System;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace CodeKata.RobotWars.Tests
{
    public class GameTests
    {
        [Fact]
        public void DeclareSquareSizedGame()
        {
            var squareGame = new Game(5, 5);
            Assert.Equal(5, squareGame.BoardSize.X);
            Assert.Equal(5, squareGame.BoardSize.Y);
        }

        [Fact]
        public void DeclareRectangularSizedGame()
        {
            var rectangularGame = new Game(3, 4);
            Assert.Equal(3, rectangularGame.BoardSize.X);
            Assert.Equal(4, rectangularGame.BoardSize.Y);
        }

        [Fact]
        public void PlaceRobot()
        {
            var game = new Game(5, 5);
            game.Take("1 2 N");

            game.WithRobots(robots =>
                {
                    Assert.Equal(1, robots[0].Position.X);
                    Assert.Equal(2, robots[0].Position.Y);
                    Assert.Equal('N', robots[0].Orientation);
                });
        }

        [Theory]
        [InlineData("L", 'W')]
        [InlineData("LL", 'S')]
        [InlineData("LLL", 'E')]
        [InlineData("LLLL", 'N')]
        [InlineData("R", 'E')]
        public void RotateRobot(String input, Char expectedDirection)
        {
            var game = new Game(5, 5);

            game.Take("1 2 N");
            game.Take(input);

            game.WithRobots(robots => Assert.Equal(expectedDirection, robots[0].Orientation));
        }

        [Fact]
        public void MoveRobot()
        {
            new[]
                {
                    new Tuple<String, Point>("M", new Point(1, 3)),
                    new Tuple<String, Point>("MMM", new Point(1, 5)),
                    new Tuple<String, Point>("RMM", new Point(3, 2)),
                    new Tuple<String, Point>("RMLMLMLM", new Point(1, 2))
                }.ToList().ForEach(useCase =>
                    {
                        var game = new Game(5, 5);

                        game.Take("1 2 N");
                        game.Take(useCase.Item1);

                        game.WithRobots(robots => Assert.Equal(useCase.Item2, robots[0].Position));
                    });
        }

        [Fact]
        public void AcceptanceTest()
        {
            var game = new Game(5, 5);
            game.Take("1 2 N");
            game.Take("LMLMLMLMM");
            game.Take("3 3 E");
            game.Take("MMRMMRMRRM");

            game.WithRobots(robots =>
                {
                    Assert.Equal("1 3 N", robots[0].ToString());
                    Assert.Equal("5 1 E", robots[1].ToString());
                });
        }

        [Fact]
        public void StateIsPreserved()
        {
            var game = new Game(5, 5);
            game.Take("0 0 N");
            game.Take("M");
            game.Take("1 1 E");
            game.Take("M");

            game.WithRobots(robots =>
                {
                    Assert.Equal("0 1 N", robots[0].ToString());
                    Assert.Equal("2 1 E", robots[1].ToString());
                    robots[0].Move();
                });

            game.WithRobots(robots => Assert.Equal("0 1 N", robots[0].ToString()));
        }
    }
}