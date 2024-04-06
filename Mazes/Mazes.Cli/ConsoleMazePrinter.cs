using System.Text;
using Mazes.Contracts;

namespace Mazes.Cli;

public class ConsoleMazePrinter: IMazePrinter
{
    private const string CornerStone = "+";
    private const string CellHorizontal = "---";
    private const string CellVertical = "|";
    private const string EmptyFloor = "   ";
    private const string LinkToSouthernCell = "   ";
    private const string LinkToEasternCell = " ";
    
    private int drawColumn;

    public void DrawMaze<T>(Cell<T>[,] cells, MazeVector startMazeVector, string title, bool drawItems = false)
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

    public void DrawCellItems<T>(Cell<T>[,] cells)
    {
        var width = cells.GetLength(0);
        var height = cells.GetLength(1);
        
        var (oldScreenPositionX, oldScreenPositionY) = Console.GetCursorPosition();
        for (int column = 0; column < width; column++)
        {
            for (int row = 0; row < height; row++)
            {
                if (cells[column,row].Item != null)
                {
                    var screenPositionX = this.drawColumn + 2 + (column) * 4;
                    var screenPositionY = (row + 2) * 2;
                    Console.SetCursorPosition(screenPositionX, screenPositionY);
                    Console.Write(cells[column, row].Item);
                }
            }
        }
            
        Console.SetCursorPosition(oldScreenPositionX, oldScreenPositionY);
    }
    
    private string GetMazeStringRepresentation<T>(Cell<T>[,] cells)
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