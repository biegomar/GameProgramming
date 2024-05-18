using System.Collections.Generic;
using System.Linq;

namespace Mazes.Contracts
{
    public class Landscape<T>
    {
        public int Width { get; }
        public int Height { get; }
        
        //public Cell<T>?[,] Cells { get; }

        public IList<Cell<T>> Cells { get; set; }

        public string Title { get; }

        private readonly IProceduralContentGenerator<T> proceduralContentGenerator;
        private readonly IContentPrinter<T> contentPrinter;

        public Landscape(CellVector dimension, IProceduralContentGenerator<T> proceduralContentGenerator, IContentPrinter<T> contentPrinter) : this(dimension,
            proceduralContentGenerator, contentPrinter, string.Empty)
        {
        }

        public Landscape(CellVector dimension, IProceduralContentGenerator<T> proceduralContentGenerator, IContentPrinter<T> contentPrinter, string title)
        {
            this.proceduralContentGenerator = proceduralContentGenerator;
            this.contentPrinter = contentPrinter;
            this.Title = title;

            this.Cells = new List<Cell<T>>();
            
            //this.Cells = new Cell<T>[dimension.X, dimension.Y];
            this.Width = dimension.X;
            this.Height = dimension.Y;
            
            InitializeMaze();
            LinkCellsInMaze();

            this.Cells = this.proceduralContentGenerator.Generate(this.Cells);
        }

        public void Draw(CellVector startCellVector)
        {
            this.contentPrinter.DrawCells(this.Cells, startCellVector, this.Title, false);
        }

        public void DrawCellItems()
        {
            this.contentPrinter.DrawCellItems(this.Cells);
        }

        public void DrawItemAtPosition(CellVector position, T item)
        {
            this.contentPrinter.DrawItemAtPosition(this.Cells, position, item);
        }

        protected virtual void InitializeMaze()
        {
            for (int column = 0; column < this.Width; column++)           
            {
                for(int row = 0; row < this.Height; row++)
                {
                    
                    this.Cells.Add(new Cell<T>(column, row));
                }
            }
        }

        private void LinkCellsInMaze()
        {            
            for (int column = 0; column < Width; column++)
            {
                for (int row = 0; row < Height; row++)
                {
                    var cellToLink = GetCellByColumnAndRow(column, row);
                    cellToLink.NorthernNeighbour = row - 1 < 0 ? null : GetCellByColumnAndRow(column, row - 1);
                    cellToLink.EasternNeighbour = column + 1 >= Width ? null : GetCellByColumnAndRow(column + 1, row);
                    cellToLink.SouthernNeighbour = row + 1 >= Height ? null : GetCellByColumnAndRow(column, row + 1);
                    cellToLink.WesternNeighbour = column - 1 < 0 ? null : GetCellByColumnAndRow(column - 1, row);
                }
            }
        }

        public void SetCellItem(CellItem<T> cellItem)
        {
            GetCellByColumnAndRow(cellItem.Position.X, cellItem.Position.Y).Item = cellItem.Item;
        }

        public void ClearCellItem(CellVector position)
        {
            GetCellByColumnAndRow(position.X, position.Y).Item = default!;
        }
        
        private Cell<T> GetCellByColumnAndRow(int column, int row)
        {
            return this.Cells.Single(cell => cell.X == column && cell.Y == row);
        }
    }
}
