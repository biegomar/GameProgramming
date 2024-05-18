using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Mazes.Contracts
{
    public interface IContentPrinter<T>
    {
        public IList<T>? Items { get; set; }
        
        public void DrawCells(IList<Cell<T>> cells, CellVector startCellVector, string title, bool drawItems = false);

        public void DrawCellItems(IList<Cell<T>> cells);

        public void DrawItemAtPosition(IList<Cell<T>> cells, CellVector position, T item);
    }
}