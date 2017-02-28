using System;

namespace CodeKata.CodeKata.TicTacToe
{
    public static class CellExtensions
    {
        public static Boolean IsInsideSecondDiagonal(this Game.Cell cell, Int32 gridSize)
        {
            return cell.ColumnIndex + cell.RowIndex == gridSize - 1;
        }

        public static Boolean IsInsideFirstDiagonal(this Game.Cell cell)
        {
            return cell.ColumnIndex == cell.RowIndex;
        }
    }
}