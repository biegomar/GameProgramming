using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazes.Contracts
{
    public class Maze
    {
        private readonly Cell[,] cells;
        private readonly IMazeGenerator mazeGenerator;

        public Maze(IMazeGenerator mazeGenerator, int width, int height)
        {
            this.mazeGenerator = mazeGenerator;
            this.cells = new Cell[width, height];
            InitializeMaze();
            LinkCellsInMaze();
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
            var dimensionZeroLength = cells.GetLength(0);
            var dimensionOneLength = cells.GetLength(1);

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
    }
}
