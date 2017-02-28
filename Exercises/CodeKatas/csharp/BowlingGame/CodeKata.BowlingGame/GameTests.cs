using Xunit;

namespace BowlingGame
{
    public class GameTests
    {
        private readonly Game _game;

        public GameTests()
        {
            _game = new Game();
        }

        [Fact]
        public void ScoreNoThrows()
        {
            Assert.Equal(0, _game.Score);
        }

        [Fact]
        public void ScoreOneThrows()
        {
            _game.RegisterThrow(1);
            Assert.Equal(1, _game.Score);
            Assert.Equal(0, _game.CurrentFrame.Index);
        }

        [Fact]
        public void ScoreTwoThrows()
        {
            _game.RegisterThrow(1);
            _game.RegisterThrow(7);
            Assert.Equal(7 + 1, _game.Score);
            Assert.Equal(1, _game.CurrentFrame.Index);
        }

        [Fact]
        public void ScoreForFrame()
        {
            _game.RegisterThrow(1);
            _game.RegisterThrow(7);
            _game.RegisterThrow(2);
            _game.RegisterThrow(6);
            _game.RegisterThrow(5);
            Assert.Equal(7 + 1, _game.GetScoreUntilFrame(0));
            Assert.Equal(7 + 1 + 2 + 6, _game.GetScoreUntilFrame(1));
            Assert.Equal(7 + 1 + 2 + 6 + 5, _game.GetScoreUntilFrame(2));
            Assert.Equal(7 + 1 + 2 + 6 + 5, _game.Score);
            Assert.Equal(2, _game.CurrentFrame.Index);
        }

        [Fact]
        public void Spare()
        {
            _game.RegisterThrow(3);
            _game.RegisterThrow(7);
            _game.RegisterThrow(2);
            Assert.Equal(7 + 3 + 2, _game.GetScoreUntilFrame(0));
            Assert.Equal(7 + 3 + 2 + 2, _game.GetScoreUntilFrame(1));
            Assert.Equal(7 + 3 + 2 + 2, _game.Score);
        }

        [Fact]
        public void SimpleStrike()
        {
            _game.RegisterThrow(10);
            _game.RegisterThrow(3);
            _game.RegisterThrow(6);
            Assert.Equal(2, _game.CurrentFrame.Index);
            Assert.Equal(10 + 3 + 6, _game.GetScoreUntilFrame(0));
            Assert.Equal(10 + 3 + 6 + 3 + 6, _game.Score);
        }

        [Fact]
        public void PerfectGame()
        {
            for (var i = 0; i < 12; i++)
                _game.RegisterThrow(10);

            Assert.Equal(300, _game.Score);
            Assert.Equal(9, _game.CurrentFrame.Index);
        }

        [Fact]
        public void SampleGame()
        {
            _game.RegisterThrow(1);
            _game.RegisterThrow(4);
            _game.RegisterThrow(4);
            _game.RegisterThrow(5);
            _game.RegisterThrow(6);
            _game.RegisterThrow(4);
            _game.RegisterThrow(5);
            _game.RegisterThrow(5);
            _game.RegisterThrow(10);
            _game.RegisterThrow(0);
            _game.RegisterThrow(1);
            _game.RegisterThrow(7);
            _game.RegisterThrow(3);
            _game.RegisterThrow(6);
            _game.RegisterThrow(4);
            _game.RegisterThrow(10);
            _game.RegisterThrow(2);
            _game.RegisterThrow(8);
            _game.RegisterThrow(6);
            Assert.Equal(133, _game.Score);
        }

        [Fact]
        public void FourThrows()
        {
            _game.RegisterThrow(1);
            _game.RegisterThrow(4);
            _game.RegisterThrow(4);
            _game.RegisterThrow(5);
            Assert.Equal(1 + 4 + 4 + 5, _game.Score);
        }

        [Fact]
        public void FourThrowsWithSpare()
        {
            _game.RegisterThrow(6);
            _game.RegisterThrow(4);
            _game.RegisterThrow(4);
            _game.RegisterThrow(5);
            Assert.Equal(6 + 4 + 4 + 4 + 5, _game.Score);
        }

        [Fact]
        public void FourThrowsWithStrike()
        {
            _game.RegisterThrow(10);
            _game.RegisterThrow(4);
            _game.RegisterThrow(4);
            _game.RegisterThrow(5);
            Assert.Equal(10 + 4 + 4 + 4 + 4 + 5, _game.Score);
        }

        [Fact]
        public void TwoStrikes()
        {
            _game.RegisterThrow(10);
            _game.RegisterThrow(10);
            Assert.Equal(10 + 10 + 10, _game.Score);
        }

        [Fact]
        public void ThreeStrikes()
        {
            _game.RegisterThrow(10);
            _game.RegisterThrow(10);
            _game.RegisterThrow(10);
            Assert.Equal(10 + 10 + 10 + 10 + 10 + 10, _game.Score);
        }

        [Fact]
        public void HeartBreak()
        {
            for (var i = 0; i < 11; i++)
                _game.RegisterThrow(10);

            _game.RegisterThrow(9);
            Assert.Equal(299, _game.Score);
        }

        [Fact]
        public void TenthFrameSpare()
        {
            for (var i = 0; i < 9; i++)
                _game.RegisterThrow(10);

            _game.RegisterThrow(9);
            _game.RegisterThrow(1);
            _game.RegisterThrow(1);
            Assert.Equal(270, _game.Score);
        }
    }
}