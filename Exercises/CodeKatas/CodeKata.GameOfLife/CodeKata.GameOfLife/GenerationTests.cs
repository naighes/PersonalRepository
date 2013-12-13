using System;
using Xunit;

namespace CodeKata.GameOfLife
{
    public class GenerationTests
    {
        [Fact]
        public void NewGeneration()
        {
            var liveCell = new CellIndex(1, 1);
            const Int32 size = 3;
            var generation = Generation.New(size, liveCell);

            for (var x = 0; x < size; x++)
                for (var y = 0; y < size; y++)
                    if (x != liveCell.X && y != liveCell.Y)
                        Assert.IsType<DeadCell>(generation.CellAt(x, y));
        }

        [Fact]
        public void LiveCellAndAllDeadExceptRightNeighbour()
        {
            var liveCells = new[]
                {
                    new CellIndex(1, 1),
                    new CellIndex(2, 1)
                };
            var generation = Generation.New(3, liveCells);

            Assert.IsType<DeadCell>(generation.Next().CellAt(1, 1));
        }

        [Fact]
        public void LiveCellAndAllDeadExceptRightAndLeftNeighbour()
        {
            var liveCells = new[]
                {
                    new CellIndex(0, 1),
                    new CellIndex(1, 1),
                    new CellIndex(2, 1)
                };
            var generation = Generation.New(3, liveCells);

            Assert.IsType<AliveCell>(generation.Next().CellAt(1, 1));
        }

        [Fact]
        public void LiveCellAndAllDeadExceptRightLeftBottomAndUpperNeighbour()
        {
            var liveCells = new[]
                {
                    new CellIndex(0, 1),
                    new CellIndex(1, 1),
                    new CellIndex(2, 1),
                    new CellIndex(1, 0),
                    new CellIndex(1, 2)
                };
            var generation = Generation.New(3, liveCells);

            Assert.IsType<DeadCell>(generation.Next().CellAt(1, 1));
        }

        [Fact]
        public void DeadCellAndAllDeadExceptRightLeftAndUpperNeighbour()
        {
            var liveCells = new[]
                {
                    new CellIndex(0, 1),
                    new CellIndex(2, 1),
                    new CellIndex(1, 0)
                };
            var generation = Generation.New(3, liveCells);

            var newGeneration = generation.Next();
            Assert.IsType<AliveCell>(newGeneration.CellAt(1, 1));
            Assert.IsType<DeadCell>(newGeneration.CellAt(0, 0));
        }
    }
}