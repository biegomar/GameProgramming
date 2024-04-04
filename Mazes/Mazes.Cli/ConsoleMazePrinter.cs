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
    
    public void DrawMaze<T>(Maze<T> maze, MazeVector startMazeVector)
    {
        this.drawColumn = startMazeVector.X;
            
        var (left, top) = Console.GetCursorPosition();
        Console.SetCursorPosition(this.drawColumn, 0);
        Console.WriteLine(maze.Title);
            
        string[] lines = GetMazeStringRepresentation(maze).Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        var newTop = 3;
        foreach (var line in lines)
        {
            var newLeft = this.drawColumn >= left ? this.drawColumn : left;
            Console.SetCursorPosition(newLeft,newTop);
            Console.WriteLine(line);
            newTop = Math.Min(newTop + 1, Console.BufferHeight - 1);
        }
    }

    public void DrawCellItems<T>(Maze<T> maze)
    {
        var (oldScreenPositionX, oldScreenPositionY) = Console.GetCursorPosition();
        for (int column = 0; column < maze.Width; column++)
        {
            for (int row = 0; row < maze.Height; row++)
            {
                if (maze.Cells[column,row].Item != null)
                {
                    var screenPositionX = this.drawColumn + 2 + (column) * 4;
                    var screenPositionY = (row + 2) * 2;
                    Console.SetCursorPosition(screenPositionX, screenPositionY);
                    Console.Write(maze.Cells[column, row].Item);
                }
            }
        }
            
        Console.SetCursorPosition(oldScreenPositionX, oldScreenPositionY);
    }
    
    private string GetMazeStringRepresentation<T>(Maze<T> maze)
    {
        var result = new StringBuilder();

        //North wall
        var segment = CornerStone + CellHorizontal;
        result.Append(string.Join("", Enumerable.Repeat(segment, maze.Width)));
        result.AppendLine(CornerStone);
            
        for (int row = 0; row < maze.Height; row++)
        {                
            var bodyRow = new StringBuilder();
            var bottomRow = new StringBuilder();

            bodyRow.Append(CellVertical);

            for (int column = 0; column < maze.Width; column++)
            {
                bodyRow.Append(EmptyFloor).Append(maze.Cells[column, row].LinkedCells.Contains(maze.Cells[column, row].EasternNeighbour) ? LinkToEasternCell : CellVertical);
                bottomRow.Append(CornerStone).Append(maze.Cells[column, row].LinkedCells.Contains(maze.Cells[column, row].SouthernNeighbour) ? LinkToSouthernCell : CellHorizontal);
            }
                
            bottomRow.Append(CornerStone);

            result.AppendLine(bodyRow.ToString());
            result.AppendLine(bottomRow.ToString());
        }
           

        return result.ToString();
    }
}