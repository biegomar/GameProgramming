using Mazes.Contracts.Cells;
using Mazes.Contracts.Dungeons;
using Mazes.Contracts.Printing;

namespace Dungeon.Cli;

public class SimpleDungeonPrinter<T>: IContentPrinter<T>
{
    private DungeonProperties properties;
    
    public IList<T>? Items { get; set; }

    public SimpleDungeonPrinter(DungeonProperties properties)
    {
        this.properties = properties;
    }
    
    public void DrawCells(IList<Cell<T>> cells, CellVector startCellVector, string title, bool drawItems = false)
    {
        this.DrawFrame();
        
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
                    Console.Write("O");
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

    private void DrawFrame()
    {
        var roomXDimension = (this.properties.PlaygroundDimension.X / this.properties.GridDimension.X) ;
        var roomYDimension = (this.properties.PlaygroundDimension.Y / this.properties.GridDimension.Y);

        for (int row = 0; row <= this.properties.PlaygroundDimension.Y; row++)
        {
            for (int column = 0; column <= this.properties.PlaygroundDimension.X; column++)
            {
                if (row % roomYDimension == 0)
                {
                    Console.SetCursorPosition(column,row);
                    Console.Write("-");
                }
                

                if (column % roomXDimension == 0)
                {
                    Console.SetCursorPosition(column,row);
                    Console.Write("|");
                }
            }
        }

        for (int row = 0; row <= this.properties.PlaygroundDimension.Y; row++)
        {
            Console.SetCursorPosition(this.properties.PlaygroundDimension.X + 2,row);
            Console.Write(row);
        }

        for (int column = 0; column <= this.properties.PlaygroundDimension.X; column++)
        {
            if (column % 5 == 0)
            {
                Console.SetCursorPosition(column,this.properties.PlaygroundDimension.Y + 2);
                Console.Write(column);    
            }
        }
    }
}