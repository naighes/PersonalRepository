using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CodeKata.GameOfLife
{
    public static class CellIndexExtensions
    {
        public static IEnumerable<Generation.ICell> AliveNeighbours(this Generation.CellIndex index, Generation generation)
        {
            return index.GetNeighbours(generation).Where(neighbour => neighbour.Status == 1);
        }

        public static IEnumerable<Generation.ICell> DeadNeighbours(this Generation.CellIndex index, Generation generation)
        {
            return index.GetNeighbours(generation).Where(neighbour => neighbour.Status == 0);
        }
    }

    public class Generation
    {
        public interface ICell
        {
            Int32 Status { get; }
            void PassJudgment(Generation generation, Action<Cell> onDeath, Action<Cell> onSpare);
        }

        public struct Cell : ICell, IEquatable<Cell>
        {
            public static ICell Empty = new EmptyCell();

            public Cell(CellIndex index, Int32 status)
            {
                _index = index;
                _status = status;
            }

            public Int32 Status
            {
                get { return _status; }
            }
            private readonly Int32 _status;

            public CellIndex Index
            {
                get { return _index; }
            }
            private readonly CellIndex _index;

            public void PassJudgment(Generation generation,
                                     Action<Cell> onDeath,
                                     Action<Cell> onSpare)
            {
                var aliveNeighboursCount = _index.AliveNeighbours(generation).Count();

                if (_status == 1)
                {
                    if (aliveNeighboursCount == 2 || aliveNeighboursCount == 3)
                        onSpare(this);
                    else
                        onDeath(this);
                }
                else
                    if (aliveNeighboursCount == 3)
                        onSpare(this);
            }

            #region Equality members

            public Boolean Equals(Cell other)
            {
                return _index.Equals(other._index);
            }

            public override Boolean Equals(Object obj)
            {
                if (ReferenceEquals(null, obj)) 
                    return false;

                return obj is Cell && Equals((Cell)obj);
            }

            public override Int32 GetHashCode()
            {
                return _index.GetHashCode();
            }

            public static Boolean operator ==(Cell left, Cell right)
            {
                return left.Equals(right);
            }

            public static Boolean operator !=(Cell left, Cell right)
            {
                return !left.Equals(right);
            }

            #endregion
        }

        public class EmptyCell : ICell
        {
            public Int32 Status
            {
                get { return -1; }
            }

            public void PassJudgment(Generation generation,
                                     Action<Cell> onDeath,
                                     Action<Cell> onSpare)
            {
            }
        }

        public struct CellIndex : IEquatable<CellIndex>
        {
            public CellIndex(Int32 x, Int32 y)
            {
                _x = x;
                _y = y;
            }

            public Int32 X
            {
                get { return _x; }
            }
            private readonly Int32 _x;

            public Int32 Y
            {
                get { return _y; }
            }
            private readonly Int32 _y;

            public ISet<ICell> GetNeighbours(Generation generation)
            {
                return new HashSet<ICell>(new List<ICell>
                    {
                        generation.CellAt(_x - 1, _y),
                        generation.CellAt(_x - 1, _y - 1),
                        generation.CellAt(_x, _y - 1),
                        generation.CellAt(_x + 1, _y - 1),
                        generation.CellAt(_x + 1, _y),
                        generation.CellAt(_x + 1, _y + 1),
                        generation.CellAt(_x, _y + 1),
                        generation.CellAt(_x - 1, _y + 1)
                    }.OfType<Cell>().Cast<ICell>());
            }

            #region Equality members

            public Boolean Equals(CellIndex other)
            {
                return X == other.X && Y == other.Y;
            }

            public override Boolean Equals(Object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;

                return obj is CellIndex && Equals((CellIndex)obj);
            }

            public override Int32 GetHashCode()
            {
                unchecked
                {
                    return (X * 397) ^ Y;
                }
            }

            public static Boolean operator ==(CellIndex left, CellIndex right)
            {
                return left.Equals(right);
            }

            public static Boolean operator !=(CellIndex left, CellIndex right)
            {
                return !left.Equals(right);
            }

            #endregion
        }

        public static Generation New(Int32 size, params CellIndex[] liveCells)
        {
            return New(size, new HashSet<CellIndex>(liveCells));
        }

        private static Generation New(Int32 size, ISet<CellIndex> liveCells)
        {
            var cells = new Dictionary<CellIndex, Int32>();

            for (var x = 0; x < size; x++)
                for (var y = 0; y < size; y++)
                {
                    var index = new CellIndex(x, y);
                    cells.Add(index, liveCells.Contains(index) ? 1 : 0);
                }

            return new Generation(cells);
        }

        public ICell CellAt(CellIndex index)
        {
            return !_cells.ContainsKey(index) ? Cell.Empty : new Cell(index, _cells[index]);
        }

        public ICell CellAt(Int32 x, Int32 y)
        {
            return CellAt(new CellIndex(x, y));
        }

        private readonly IDictionary<CellIndex, Int32> _cells;

        private Generation(IDictionary<CellIndex, Int32> cells)
        {
            _cells = cells;
        }

        public Generation Next()
        {
            var result = new Dictionary<CellIndex, Int32>();

            foreach (var cell in _cells)
                CellAt(cell.Key).PassJudgment(this,
                                              c => result.Add(c.Index, 0),
                                              c => result.Add(c.Index, 1));

            return new Generation(result);
        }
    }

    public class Kata
    {
        [Fact]
        public void NewGeneration()
        {
            var liveCell = new Generation.CellIndex(1, 1);
            const Int32 size = 3;
            var generation = Generation.New(size, liveCell);

            Assert.Equal(1, generation.CellAt(liveCell.X, liveCell.Y).Status);

            for (var x = 0; x < size; x++)
                for (var y = 0; y < size; y++)
                    if (x != liveCell.X && y != liveCell.Y)
                        Assert.Equal(0, generation.CellAt(x, y).Status);
        }

        [Fact]
        public void LiveCellAndAllDeadExceptRightNeighbour()
        {
            var liveCells = new[]
                {
                    new Generation.CellIndex(1, 1),
                    new Generation.CellIndex(2, 1)
                };
            var generation = Generation.New(3, liveCells);

            Assert.Equal(0, generation.Next().CellAt(1, 1).Status);
        }

        [Fact]
        public void LiveCellAndAllDeadExceptRightAndLeftNeighbour()
        {
            var liveCells = new[]
                {
                    new Generation.CellIndex(0, 1),
                    new Generation.CellIndex(1, 1),
                    new Generation.CellIndex(2, 1)
                };
            var generation = Generation.New(3, liveCells);

            Assert.Equal(1, generation.Next().CellAt(1, 1).Status);
        }

        [Fact]
        public void LiveCellAndAllDeadExceptRightLeftBottomAndUpperNeighbour()
        {
            var liveCells = new[]
                {
                    new Generation.CellIndex(0, 1),
                    new Generation.CellIndex(1, 1),
                    new Generation.CellIndex(2, 1),
                    new Generation.CellIndex(1, 0),
                    new Generation.CellIndex(1, 2)
                };
            var generation = Generation.New(3, liveCells);

            Assert.Equal(0, generation.Next().CellAt(1, 1).Status);
        }

        [Fact]
        public void DeadCellAndAllDeadExceptRightLeftAndUpperNeighbour()
        {
            var liveCells = new[]
                {
                    new Generation.CellIndex(0, 1), // Left
                    new Generation.CellIndex(2, 1), // Right
                    new Generation.CellIndex(1, 0) // Upper
                };
            var generation = Generation.New(3, liveCells);

            Assert.Equal(1, generation.Next().CellAt(1, 1).Status);
        }
    }
}