using System.Text;
using Mazes.Contracts;

namespace NearlyRogue.Cli;

public class ConsoleMazePrinter<T>(IList<T>? printableItems = null) : IMazePrinter<T>
{
    private const string CornerStone = "+";
    private const string CellHorizontal = "---";
    private const string CellVertical = "|";
    private const string EmptyFloor = "   ";
    private const string LinkToSouthernCell = "   ";
    private const string LinkToEasternCell = " ";
    
    private int drawColumn;

    public IList<T>? Items { get; set; } = printableItems;

    public void DrawMaze(Cell<T>?[,] cells, MazeVector startMazeVector, string title, bool drawItems = false)
    { 
        this.drawColumn = startMazeVector.X;
            
        var (left, top) = Console.GetCursorPosition();
        Console.SetCursorPosition(this.drawColumn, 0);
        Console.WriteLine(title);
            
        string[] lines = GetMazeStringRepresentation(cells).Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        var newTop = 3;
        foreach (var line in lines)
        {
            var newLeft = this.drawColumn >= left ? this.drawColumn : left;
            Console.SetCursorPosition(newLeft,newTop);
            Console.WriteLine(line);
            newTop = Math.Min(newTop + 1, Console.BufferHeight - 1);
        }

        if (drawItems)
        {
            this.DrawCellItems(cells);
        }
    }

    public void DrawCellItems(Cell<T>?[,] cells)
    {
        var width = cells.GetLength(0);
        var height = cells.GetLength(1);
        
        var (oldScreenPositionX, oldScreenPositionY) = Console.GetCursorPosition();
        for (int column = 0; column < width; column++)
        {
            for (int row = 0; row < height; row++)
            {
                var screenPositionX = this.drawColumn + 2 + (column) * 4;
                var screenPositionY = (row + 2) * 2;
                Console.SetCursorPosition(screenPositionX, screenPositionY);
                
                if (cells[column,row].Item != null)
                {
                    Console.Write(cells[column, row].Item);
                }
                else
                {
                   Console.Write(' '); 
                }
            }
        }
            
        Console.SetCursorPosition(oldScreenPositionX, oldScreenPositionY);
    }

    public void DrawItemAtPosition(Cell<T>?[,] cells, MazeVector position, T item)
    {
        int oldX = Console.CursorLeft;
        int oldY = Console.CursorTop;
        var screenPositionX = this.drawColumn + 2 + (position.X) * 4;
        var screenPositionY = (position.Y + 2) * 2;
        Console.SetCursorPosition(screenPositionX, screenPositionY);
        Console.Write(item);
        Console.SetCursorPosition(oldX, oldY);
    }

    private string GetMazeStringRepresentation<T>(Cell<T>?[,] cells)
    {
        var result = new StringBuilder();
        
        var width = cells.GetLength(0);
        var height = cells.GetLength(1);

        //North wall
        var segment = CornerStone + CellHorizontal;
        result.Append(string.Join("", Enumerable.Repeat(segment, width)));
        result.AppendLine(CornerStone);
            
        for (int row = 0; row < height; row++)
        {                
            var bodyRow = new StringBuilder();
            var bottomRow = new StringBuilder();

            bodyRow.Append(CellVertical);

            for (int column = 0; column < width; column++)
            {
                bodyRow.Append(EmptyFloor).Append(cells[column, row].LinkedCells.Contains(cells[column, row].EasternNeighbour) ? LinkToEasternCell : CellVertical);
                bottomRow.Append(CornerStone).Append(cells[column, row].LinkedCells.Contains(cells[column, row].SouthernNeighbour) ? LinkToSouthernCell : CellHorizontal);
            }
                
            bottomRow.Append(CornerStone);

            result.AppendLine(bodyRow.ToString());
            result.AppendLine(bottomRow.ToString());
        }
           

        return result.ToString();
    }
}