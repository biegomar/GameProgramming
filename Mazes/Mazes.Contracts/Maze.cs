using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Mazes.Contracts
{
    public class Maze
    {
        private const string CornerStone = "+";
        private const string CellHorizontal = "---";
        private const string CellVertical = "|";
        private const string EmptyFloor = "   ";
        private const string LinkToSouthernCell = "   ";
        private const string LinkToEasternCell = " ";

        private readonly Cell[,] cells;
        private readonly IMazeGenerator mazeGenerator;
        private readonly int dimensionZeroLength;
        private readonly int dimensionOneLength;

        public Maze(IMazeGenerator mazeGenerator, int width, int height)
        {
            this.mazeGenerator = mazeGenerator;

            this.cells = new Cell[width, height];
            this.dimensionZeroLength = cells.GetLength(0);
            this.dimensionOneLength = cells.GetLength(1);

            InitializeMaze();
            LinkCellsInMaze();

            this.cells = this.mazeGenerator.Generate(this.cells);
        }

        private void InitializeMaze()
        {
            for (int row = 0; row < cells.GetLength(0); row++)           
            {
                for(int column = 0; column < cells.GetLength(1); column++)
                {
                    this.cells[row, column] = new Cell();
                }
            }
        }

        private void LinkCellsInMaze()
        {            
            for (int row = 0; row < dimensionZeroLength; row++)
            {
                for (int column = 0; column < dimensionOneLength; column++)
                {
                    this.cells[row, column].NothernNeighbour = row - 1 < 0 ? null : this.cells[row - 1, column];
                    this.cells[row, column].EasternNeighbour = column + 1 >= dimensionOneLength ? null : this.cells[row, column + 1];
                    this.cells[row, column].SouthernNeighbour = row + 1 >= dimensionZeroLength ? null : this.cells[row + 1, column];
                    this.cells[row, column].WesternNeighbour = column - 1 < 0 ? null : this.cells[row, column - 1];
                }
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            //North wall
            var segment = CornerStone + CellHorizontal;
            result.Append(string.Join("", Enumerable.Repeat(segment, dimensionZeroLength)));
            result.AppendLine(CornerStone);

            for (int row = 0; row < dimensionZeroLength; row++)
            {                
                var bodyRow = new StringBuilder();
                var bottomRow = new StringBuilder();

                bodyRow.Append(CellVertical);

                for (int column = 0; column < dimensionOneLength; column++)
                {
                    bodyRow.Append(EmptyFloor).Append(this.cells[row, column].LinkedCells.Contains(this.cells[row, column].EasternNeighbour) ? LinkToEasternCell : CellVertical);
                    bottomRow.Append(CornerStone).Append(this.cells[row, column].LinkedCells.Contains(this.cells[row, column].SouthernNeighbour) ? LinkToSouthernCell : CellHorizontal);
                }
                
                bottomRow.Append(CornerStone);

                result.AppendLine(bodyRow.ToString());
                result.AppendLine(bottomRow.ToString());
            }
           

            return result.ToString();
        }
    }
}
