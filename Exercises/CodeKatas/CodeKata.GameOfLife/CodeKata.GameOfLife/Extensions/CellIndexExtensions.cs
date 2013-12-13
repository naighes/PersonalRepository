using System.Collections.Generic;
using System.Linq;

namespace CodeKata.GameOfLife.Extensions
{
    public static class CellIndexExtensions
    {
        public static IEnumerable<ICell> AliveNeighbours(this CellIndex index, Generation generation)
        {
            return index.GetNeighbours(generation).OfType<AliveCell>().Cast<ICell>();
        }
    }
}