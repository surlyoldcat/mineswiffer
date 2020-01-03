using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace mineswiffer
{
    public class Board
    {
        private static readonly Random rnd = new Random();
        private List<Cell> cells = null;
        private int numRows = 0;
        private int numCols = 0;
        private const string DELIMITER = "  ";

        public int MinesRemaining {get; private set;}

        protected Board(int rows, int cols, int numMines)
        {
            MinesRemaining = numMines;
            numRows = rows;
            numCols = cols;    
            int boardLength = numRows * numCols;
            cells = new List<Cell>(boardLength);
            var mines = new List<int>(numMines);
            for (int i=0; i < numMines; i++)
            {
                //index 0 should never have a mine (it's the default first move)
                mines.Add(rnd.Next(1, boardLength - 1));
            }

            for (int j=0; j < boardLength; j++)
            {
                bool mined = mines.Contains(j);
                cells.Add(new Cell(mined));
            }
        }

        public MoveResultEnum MakeMove(int row, int col)
        {
            int rowIdx = row - 1;
            int colIdx = col - 1;
            var cell = GetCellByIndices(rowIdx, colIdx);
            if (cell.IsRevealed)
            {
                return MoveResultEnum.Invalid;
            }
            if (cell.HasMine)
            {
                return MoveResultEnum.FieryDeath;
            }
            cell.Reveal();
            foreach(Cell c in GetNeighborsOf(rowIdx, colIdx))
            {
                c.Reveal();
            }
            MinesRemaining = cells.Count(c => c.HasMine && !c.IsRevealed);
            return MoveResultEnum.Cleared;

        }

        public void Print(TextWriter writer)
        {

            for (int row = 0; row <= numRows; row++)
            {
                writer.WriteLine(BuildRow(row));
                
            }
        }

        private string BuildRow(int rownum)
        {
            if (rownum == 0)
            {
                string colNumbers = String.Join(DELIMITER, Enumerable.Range(1, numCols));
                return " " + DELIMITER + colNumbers;
            }
            int rowStartIdx = (rownum - 1) * numCols;
            var rowValues = cells.GetRange(rowStartIdx, numCols).Select(c => c.ToString());
            var line = String.Format("{0}{1}{2}", rownum, DELIMITER, String.Join(DELIMITER, rowValues));
            return line;

        }

        private IEnumerable<Cell> GetNeighborsOf(int rowIdx, int colIdx)
        {
            var neighbors = new List<Cell>(4);
            //above
            if (rowIdx > 0)
            {
                neighbors.Add(GetCellByIndices(rowIdx - 1, colIdx));
            }
            //below
            if (rowIdx < numRows - 1)
            {
                neighbors.Add(GetCellByIndices(rowIdx + 1, colIdx));
            }
            //left
            if (colIdx > 0)
            {
                neighbors.Add(GetCellByIndices(rowIdx, colIdx - 1));
            }
            //right
            if (colIdx < numCols - 1)
            {
                neighbors.Add(GetCellByIndices(rowIdx, colIdx + 1));
            }
            return neighbors;
        }

        private Cell GetCellByIndices(int rowIdx, int colIdx)
        {
            int index = (numCols * rowIdx) + colIdx;
            return cells[index];
        }

        public static Board Create(int rows, int cols, int mines)
        {
            Board b = new Board(rows, cols, mines);
            b.MakeMove(1, 1);
            return b;
        }
    }
}