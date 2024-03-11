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
            for (int column = 0; column < cells.GetLength(0); column++)           
            {
                for(int row = 0; row < cells.GetLength(1); row++)
                {
                    this.cells[column, row] = new Cell();
                }
            }
        }

        private void LinkCellsInMaze()
        {            
            for (int column = 0; column < dimensionZeroLength; column++)
            {
                for (int row = 0; row < dimensionOneLength; row++)
                {
                    this.cells[column, row].NothernNeighbour = row - 1 < 0 ? null : this.cells[column, row - 1];
                    this.cells[column, row].EasternNeighbour = column + 1 >= dimensionZeroLength ? null : this.cells[column + 1, row];
                    this.cells[column, row].SouthernNeighbour = row + 1 >= dimensionOneLength ? null : this.cells[column, row + 1];
                    this.cells[column, row].WesternNeighbour = column - 1 < 0 ? null : this.cells[column - 1, row];
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
            
            for (int row = 0; row < dimensionOneLength; row++)
            {                
                var bodyRow = new StringBuilder();
                var bottomRow = new StringBuilder();

                bodyRow.Append(CellVertical);

                for (int column = 0; column < dimensionZeroLength; column++)
                {
                    bodyRow.Append(EmptyFloor).Append(this.cells[column, row].LinkedCells.Contains(this.cells[column, row].EasternNeighbour) ? LinkToEasternCell : CellVertical);
                    bottomRow.Append(CornerStone).Append(this.cells[column, row].LinkedCells.Contains(this.cells[column, row].SouthernNeighbour) ? LinkToSouthernCell : CellHorizontal);
                }
                
                bottomRow.Append(CornerStone);

                result.AppendLine(bodyRow.ToString());
                result.AppendLine(bottomRow.ToString());
            }
           

            return result.ToString();
        }
    }
}
