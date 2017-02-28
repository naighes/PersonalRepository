using System;
using System.Collections.Generic;
using System.Text;

namespace CodeKata.CodeKata.TicTacToe
{
    public class Game
    {
        private const Int32 BoardSize = 3;
        private readonly String _playerOneName;
        private readonly String _playerTwoName;

        public Game(String playerOneName, String playerTwoName)
        {
            _board = new Board(BoardSize);
            _playerTwoName = playerTwoName;
            _currentPlayer = _playerOneName = playerOneName;
        }

        private readonly Board _board;

        public String CurrentPlayerName
        {
            get { return _currentCell.OccupiedBy; }
        }

        public String WinnerPlayerName
        {
            get { return _winnerPlayerName; }
        }
        private String _winnerPlayerName;

        private readonly ISet<Func<Cell, Board, Boolean>> _winningRules = new HashSet<Func<Cell, Board, Boolean>>(new Func<Cell, Board, Boolean>[]
            {
                (cell, board) => board.FillsFirstDiagonal(cell),
                (cell, board) => board.FillsSecondDiagonal(cell),
                (cell, board) => board.FillsVertical(cell),
                (cell, board) => board.FillsHorizontal(cell)
            });

        private Cell _currentCell;
        private String _currentPlayer;

        private void CheckForWinner()
        {
            foreach (var rule in _winningRules)
            {
                if (!rule(_currentCell, _board))
                    continue;

                _winnerPlayerName = _currentCell.OccupiedBy;
                break;
            }
        }

        private void MarkCell(Int32 rowIndex, Int32 columnIndex)
        {
            _currentCell = _board[rowIndex, columnIndex];
            _currentCell.OccupiedBy = _currentPlayer;
            _currentPlayer = _currentPlayer == _playerOneName ? _playerTwoName : _playerOneName;
        }

        public void Put(Int32 rowIndex, Int32 columnIndex)
        {
            MarkCell(rowIndex, columnIndex);
            CheckForWinner();
        }

        public class Board
        {
            public Board(Int32 size)
            {
                _size = size;
                _grid = new Cell[size, size];

                for (var i = 0; i < size; i++)
                    for (var j = 0; j < size; j++)
                        _grid[i, j] = new Cell(i, j);
            }

            private readonly Cell[,] _grid;

            public Cell this[Int32 rowIndex, Int32 columnIndex]
            {
                get { return _grid[rowIndex, columnIndex]; }
            }

            private readonly Int32 _size;

            internal Boolean IsInside(Int32 rowIndex, Int32 columnIndex)
            {
                return columnIndex >= 0 && columnIndex < _size &&
                       rowIndex >= 0 && rowIndex < _size;
            }

            public Boolean FillsFirstDiagonal(Cell cell)
            {
                if (!cell.IsInsideFirstDiagonal())
                    return false;

                var counter = Count(cell, (c, b) => c.MoveTopLeft(b));
                counter += Count(cell, (c, b) => c.MoveBottomRight(b));

                return counter == _size - 1;
            }

            public Boolean FillsSecondDiagonal(Cell cell)
            {
                if (!cell.IsInsideSecondDiagonal(_size))
                    return false;

                var counter = Count(cell, (c, b) => c.MoveTopRight(b));
                counter += Count(cell, (c, b) => c.MoveBottomLeft(b));

                return counter == _size - 1;
            }

            public Boolean FillsVertical(Cell cell)
            {
                var counter = Count(cell, (c, b) => c.MoveTop(b));
                counter += Count(cell, (c, b) => c.MoveBottom(b));

                return counter == _size - 1;
            }

            public Boolean FillsHorizontal(Cell cell)
            {
                var counter = Count(cell, (c, b) => c.MoveLeft(b));
                counter += Count(cell, (c, b) => c.MoveRight(b));

                return counter == _size - 1;
            }

            private Int32 Count(Cell cell, Func<Cell, Board, Cell> func)
            {
                var counter = 0;
                var inspectedCell = cell;

                while ((inspectedCell = func(inspectedCell, this)) != null)
                    if (inspectedCell.OccupiedBy == cell.OccupiedBy)
                        counter++;

                return counter;
            }

            public override String ToString()
            {
                var sb = new StringBuilder();

                for (var i = 0; i < _size; i++)
                {
                    for (var j = 0; j < _size; j++)
                        sb.Append(this[i, j]);

                    sb.Append(Environment.NewLine);
                }

                return sb.ToString();
            }
        }

        public class Cell
        {
            public Cell(Int32 rowIndex, Int32 columnIndex)
            {
                _rowIndex = rowIndex;
                _columnIndex = columnIndex;
            }

            public String OccupiedBy { get; internal set; }

            public Int32 RowIndex
            {
                get { return _rowIndex; }
            }
            private readonly Int32 _rowIndex;

            public Int32 ColumnIndex
            {
                get { return _columnIndex; }
            }
            private readonly Int32 _columnIndex;

            public Cell MoveTopLeft(Board board)
            {
                return Move(board, RowIndex - 1, ColumnIndex - 1);
            }

            public Cell MoveBottomRight(Board board)
            {
                return Move(board, RowIndex + 1, ColumnIndex + 1);
            }

            public Cell MoveTopRight(Board board)
            {
                return Move(board, RowIndex - 1, ColumnIndex + 1);
            }

            public Cell MoveBottomLeft(Board board)
            {
                return Move(board, RowIndex + 1, ColumnIndex - 1);
            }

            public Cell MoveBottom(Board board)
            {
                return Move(board, RowIndex + 1, ColumnIndex);
            }

            public Cell MoveTop(Board board)
            {
                return Move(board, RowIndex - 1, ColumnIndex);
            }

            public Cell MoveLeft(Board board)
            {
                return Move(board, RowIndex, ColumnIndex - 1);
            }

            public Cell MoveRight(Board board)
            {
                return Move(board, RowIndex, ColumnIndex + 1);
            }

            private static Cell Move(Board board, Int32 rowIndex, Int32 columnIndex)
            {
                return !board.IsInside(rowIndex, columnIndex)
                           ? null
                           : board[rowIndex, columnIndex];
            }

            public override String ToString()
            {
                return String.Format("[{0},{1}]", _rowIndex, _columnIndex);
            }
        }
    }
}