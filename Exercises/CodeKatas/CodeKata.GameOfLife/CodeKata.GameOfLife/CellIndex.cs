using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeKata.GameOfLife
{
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
                }.OfType<AliveCell>().Cast<ICell>());
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

        #endregion

        public override String ToString()
        {
            return String.Format("{0}:{1}", _x, _y);
        }
    }
}