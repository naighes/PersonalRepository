using System;

namespace CodeKata.GameOfLife
{
    public interface ICell
    {
        CellIndex Index { get; }
        void PassJudgment(Generation generation, Action<ICell> onDeath, Action<ICell> onSpare);
    }
}