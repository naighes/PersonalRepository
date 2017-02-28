using Xunit;

namespace CodeKata.CodeKata.TicTacToe
{
    public class TicTacToeTests
    {
        [Fact]
        public void CurrentPlayer()
        {
            var game = new Game("Nicola", "Mario");
            game.Put(1, 1);
            Assert.Equal("Nicola", game.CurrentPlayerName);
            game.Put(0, 1);
            Assert.Equal("Mario", game.CurrentPlayerName);
        }

        [Fact]
        public void FirstDiagonalWins()
        {
            var game = new Game("Nicola", "Mario");
            game.Put(0, 0);
            game.Put(1, 0);
            game.Put(1, 1);
            game.Put(2, 0);
            game.Put(2, 2);
            Assert.Equal("Nicola", game.WinnerPlayerName);
        }

        [Fact]
        public void SecondDiagonalWins()
        {
            var game = new Game("Nicola", "Mario");
            game.Put(0, 2);
            game.Put(0, 1);
            game.Put(1, 1);
            game.Put(2, 2);
            game.Put(2, 0);
            Assert.Equal("Nicola", game.WinnerPlayerName);
        }

        [Fact]
        public void ThreePutsNoWins()
        {
            var game = new Game("Nicola", "Mario");
            game.Put(0, 0);
            game.Put(1, 0);
            game.Put(1, 1);
            Assert.Null(game.WinnerPlayerName);
        }

        [Fact]
        public void VerticalRow()
        {
            var game = new Game("Nicola", "Mario");
            game.Put(0, 0);
            game.Put(0, 2);
            game.Put(1, 0);
            game.Put(1, 2);
            game.Put(2, 0);
            Assert.Equal("Nicola", game.WinnerPlayerName);
        }

        [Fact]
        public void HorizontalRow()
        {
            var game = new Game("Nicola", "Mario");
            game.Put(0, 0);
            game.Put(1, 1);
            game.Put(0, 1);
            game.Put(2, 2);
            game.Put(0, 2);
            Assert.Equal("Nicola", game.WinnerPlayerName);
        }
    }
}