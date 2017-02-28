using System;
using System.Collections.Generic;

namespace CodeKata.GameOfLife
{
    public class Generation
    {
        public class EmptyCell : ICell
        {
            public CellIndex Index
            {
                get { return new CellIndex(-1, -1); }
            }

            public void PassJudgment(Generation generation,
                                     Action<ICell> onDeath,
                                     Action<ICell> onSpare)
            {
            }
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

        private ICell CellAt(CellIndex index)
        {
            if (!_cells.ContainsKey(index)) 
                return new EmptyCell();

            if (_cells[index] == 0)
                return new DeadCell(index);

            return new AliveCell(index);
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
}