using System.Collections.Generic;
using System.Linq;

namespace Mazes.Contracts
{
    public class Landscape<T>
    {
        public int Width { get; }
        public int Height { get; }

        public IList<Cell<T>> Cells { get; private set; }

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
            
            this.Width = dimension.X;
            this.Height = dimension.Y;
            
            this.InitializeStructure();

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

        public void SetCellItem(CellItem<T> cellItem)
        {
            GetCellByColumnAndRow(cellItem.Position.X, cellItem.Position.Y).Item = cellItem.Item;
        }

        public void ClearCellItem(CellVector position)
        {
            GetCellByColumnAndRow(position.X, position.Y).Item = default!;
        }
        
        private void InitializeCells()
        {
            this.Cells = proceduralContentGenerator.InitializeCells(this.Cells);
        }
        
        private void LinkCells()
        {
            this.Cells = proceduralContentGenerator.LinkCells(this.Cells);
        }
        
        private void InitializeStructure()
        {
            InitializeCells();
            LinkCells();
        }
        
        private Cell<T> GetCellByColumnAndRow(int column, int row)
        {
            return this.Cells.Single(cell => cell.X == column && cell.Y == row);
        }
    }
}
