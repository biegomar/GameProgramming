using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Mazes.Contracts
{
    public interface IContentPrinter<T>
    {
        public IList<T>? Items { get; set; }
        
        public void DrawCells(Cell<T>?[,] cells, MazeVector startMazeVector, string title, bool drawItems = false);

        public void DrawCellItems(Cell<T>?[,] cells);

        public void DrawItemAtPosition(Cell<T>?[,] cells, MazeVector position, T item);
    }
}