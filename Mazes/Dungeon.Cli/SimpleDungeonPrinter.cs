using Mazes.Contracts.Cells;
using Mazes.Contracts.Printing;

namespace Dungeon.Cli;

public class SimpleDungeonPrinter<T>: IContentPrinter<T>
{
    public IList<T>? Items { get; set; }
    public void DrawCells(IList<Cell<T>> cells, CellVector startCellVector, string title, bool drawItems = false)
    {
        var width = cells.Max(cell => cell.X) + 1;
        var height = cells.Max(cell => cell.Y) + 1;
            
        for (int column = 0; column < width; column++)           
        {
            for(int row = 0; row < height; row++)
            {
                var cell = this.GetCellByColumnAndRow(cells, column, row);
                if (cell.IsVisible)
                {
                    Console.SetCursorPosition(column,row);
                    Console.Write("X");
                }
            }
        }
    }

    public void DrawCellItems(IList<Cell<T>> cells)
    {
        throw new NotImplementedException();
    }

    public void DrawItemAtPosition(IList<Cell<T>> cells, CellVector position, T item)
    {
        throw new NotImplementedException();
    }
    
    private Cell<T> GetCellByColumnAndRow(IList<Cell<T>> cells, int column, int row)
    {
        return cells.Single(cell => cell.X == column && cell.Y == row);
    }
}