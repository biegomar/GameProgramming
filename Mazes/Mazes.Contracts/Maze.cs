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
        private readonly IMazeGenerator mazeGenerator;
        private readonly IMazePrinter mazePrinter;

        public int Width { get; }
        public int Height { get; }
        
        public Cell[,] Cells { get; }
        
        public string Title { get; }

        public Maze(MazeVector dimension, IMazeGenerator mazeGenerator, IMazePrinter mazePrinter) : this(dimension,mazeGenerator, mazePrinter, string.Empty)
        {
        }

        public Maze(MazeVector dimension, IMazeGenerator mazeGenerator, IMazePrinter mazePrinter, string title)
        {
            this.mazeGenerator = mazeGenerator;
            this.mazePrinter = mazePrinter;

            this.Cells = new Cell[dimension.X, dimension.Y];
            this.Width = Cells.GetLength(0);
            this.Height = Cells.GetLength(1);
            this.Title = title;

            InitializeMaze();
            LinkCellsInMaze();

            this.Cells = this.mazeGenerator.Generate(this.Cells);
        }

        public void Draw(MazeVector startMazeVector)
        {
            this.mazePrinter.DrawMaze(this, startMazeVector);
        }

        public void DrawCellItems()
        {
            this.mazePrinter.DrawCellItems(this);
        }

        private void InitializeMaze()
        {
            for (int column = 0; column < Cells.GetLength(0); column++)           
            {
                for(int row = 0; row < Cells.GetLength(1); row++)
                {
                    this.Cells[column, row] = new Cell();
                }
            }
        }

        private void LinkCellsInMaze()
        {            
            for (int column = 0; column < Width; column++)
            {
                for (int row = 0; row < Height; row++)
                {
                    this.Cells[column, row].NothernNeighbour = row - 1 < 0 ? null : this.Cells[column, row - 1];
                    this.Cells[column, row].EasternNeighbour = column + 1 >= Width ? null : this.Cells[column + 1, row];
                    this.Cells[column, row].SouthernNeighbour = row + 1 >= Height ? null : this.Cells[column, row + 1];
                    this.Cells[column, row].WesternNeighbour = column - 1 < 0 ? null : this.Cells[column - 1, row];
                }
            }
        }

        public void SetCellItem(CellItem cellItem)
        {
            this.Cells[cellItem.Position.X, cellItem.Position.Y].Item = cellItem.Item;
        }
    }
}
