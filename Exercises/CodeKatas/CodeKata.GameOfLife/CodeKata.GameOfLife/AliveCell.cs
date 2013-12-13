using System;
using System.Linq;
using CodeKata.GameOfLife.Extensions;

namespace CodeKata.GameOfLife
{
    public struct AliveCell : ICell, IEquatable<ICell>
    {
        public AliveCell(CellIndex index)
        {
            _index = index;
        }

        public CellIndex Index
        {
            get { return _index; }
        }
        private readonly CellIndex _index;

        public void PassJudgment(Generation generation,
                                 Action<ICell> onDeath,
                                 Action<ICell> onSpare)
        {
            var aliveNeighboursCount = _index.AliveNeighbours(generation).Count();

            if (aliveNeighboursCount == 2 || aliveNeighboursCount == 3)
                onSpare(this);
            else
                onDeath(this);
        }

        #region Equality members

        public Boolean Equals(ICell other)
        {
            return _index.Equals(other.Index);
        }

        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;

            return obj is ICell && Equals((ICell)obj);
        }

        public override Int32 GetHashCode()
        {
            return _index.GetHashCode();
        }

        #endregion
    }
}