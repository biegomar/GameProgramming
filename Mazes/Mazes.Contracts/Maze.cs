using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Mazes.Contracts
{
    public class Maze : IMazePrinter
    {
        private const string CornerStone = "+";
        private const string CellHorizontal = "---";
        private const string CellVertical = "|";
        private const string EmptyFloor = "   ";
        private const string FloorWithItem = " {0} ";
        private const string LinkToSouthernCell = "   ";
        private const string LinkToEasternCell = " ";

        
        private readonly IMazeGenerator mazeGenerator;

        private int drawColumn;

        public int Width { get; }
        public int Height { get; }
        
        public Cell[,] Cells { get; }
        
        public string Title { get; }

        public Maze(IMazeGenerator mazeGenerator, MazeVector dimension) : this(mazeGenerator, dimension,string.Empty)
        {
        }

        public Maze(IMazeGenerator mazeGenerator, MazeVector dimension, string title)
        {
            this.mazeGenerator = mazeGenerator;

            this.Cells = new Cell[dimension.X, dimension.Y];
            this.Width = Cells.GetLength(0);
            this.Height = Cells.GetLength(1);
            this.Title = title;

            InitializeMaze();
            LinkCellsInMaze();

            this.Cells = this.mazeGenerator.Generate(this.Cells);
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
            this.Cells[cellItem.posX, cellItem.posY].Item = cellItem.item;
        }
        

        public override string ToString()
        {
            var result = new StringBuilder();

            //North wall
            var segment = CornerStone + CellHorizontal;
            result.Append(string.Join("", Enumerable.Repeat(segment, Width)));
            result.AppendLine(CornerStone);
            
            for (int row = 0; row < Height; row++)
            {                
                var bodyRow = new StringBuilder();
                var bottomRow = new StringBuilder();

                bodyRow.Append(CellVertical);

                for (int column = 0; column < Width; column++)
                {
                    var floorItem = this.Cells[column, row].Item != null ? string.Format(FloorWithItem, this.Cells[column, row].Item) : EmptyFloor;
                    bodyRow.Append(floorItem).Append(this.Cells[column, row].LinkedCells.Contains(this.Cells[column, row].EasternNeighbour) ? LinkToEasternCell : CellVertical);
                    bottomRow.Append(CornerStone).Append(this.Cells[column, row].LinkedCells.Contains(this.Cells[column, row].SouthernNeighbour) ? LinkToSouthernCell : CellHorizontal);
                }
                
                bottomRow.Append(CornerStone);

                result.AppendLine(bodyRow.ToString());
                result.AppendLine(bottomRow.ToString());
            }
           

            return result.ToString();
        }

        public void DrawMaze(MazeVector startMazeVector)
        {
            this.drawColumn = startMazeVector.X;
            
            var (left, top) = Console.GetCursorPosition();
            Console.SetCursorPosition(this.drawColumn, 0);
            Console.WriteLine(this.Title);
            
            string[] lines = this.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var newTop = 3;
            foreach (var line in lines)
            {
                var newLeft = this.drawColumn >= left ? this.drawColumn : left;
                Console.SetCursorPosition(newLeft,newTop);
                Console.WriteLine(line);
                newTop = Math.Min(newTop + 1, Console.BufferHeight - 1);
            }
        }

        public void RedrawMaze()
        {
            var (oldScreenPositionX, oldScreenPositionY) = Console.GetCursorPosition();
            for (int column = 0; column < Width; column++)
            {
                for (int row = 0; row < Height; row++)
                {
                    if (this.Cells[column,row].Item != null)
                    {
                        var screenPositionX = this.drawColumn + 2 + (column) * 4;
                        var screenPositionY = (row + 2) * 2;
                        Console.SetCursorPosition(screenPositionX, screenPositionY);
                        Console.Write(this.Cells[column, row].Item);
                    }
                }
            }
            
            Console.SetCursorPosition(oldScreenPositionX, oldScreenPositionY);
        }
    }
}
