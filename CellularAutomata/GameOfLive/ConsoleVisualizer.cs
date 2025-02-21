using System.Text;
using CellularAutomata;

namespace GameOfLive;

public static class ConsoleVisualizer<T>
{
    static int counter = 0;
    public static void Render(PlayGround<T> playGround, Func<T, char> stateToChar)
    {
        Console.SetCursorPosition(0, 0);
        Console.Write($"Generation {counter++}");
        
        var startX = 0;
        var startY = 2;
        
        
        var (actualX, actualY) = Console.GetCursorPosition();
        
        for (var y = 0; y < playGround.Dimension.Y; y++)
        {
            var lineBuilder = new StringBuilder();
            
            for (var x = 0; x < playGround.Dimension.X; x++)
            {
                var positionToCheck = new Vector(x, y);
                lineBuilder.Append(stateToChar(playGround[positionToCheck]));
            }
            
            Console.SetCursorPosition(startX, startY + y);
            Console.Write(lineBuilder.ToString());
        }
        
        Console.SetCursorPosition(actualX, actualY);
    }

    public static void RenderWithColors(PlayGround<T> playGround, Func<T, ConsoleColor> stateToColor)
    {
        Console.SetCursorPosition(0, 0);
        Console.Write($"Generation {counter++}");

        var startX = 0;
        var startY = 2;

        var (actualX, actualY) = Console.GetCursorPosition();

        for (int y = 0; y < playGround.Dimension.Y; y++)
        {
            var currentColor = ConsoleColor.Black;
            var lineBuilder = new StringBuilder();

            for (int x = 0; x < playGround.Dimension.X; x++)
            {
                var positionToCheck = new Vector(x, y);
                var cellColor = stateToColor(playGround[positionToCheck]);

                // Wenn es ein neuer Farbbereich ist, wird die bisherige Farbe ausgegeben
                if (lineBuilder.Length > 0 && cellColor != currentColor)
                {
                    PrintColoredLine(lineBuilder.ToString(), startX + (x - lineBuilder.Length), startY + y,
                        currentColor);
                    lineBuilder.Clear();
                }
                
                currentColor = cellColor;
                lineBuilder.Append(' ');
            }
            
            if (lineBuilder.Length > 0)
            {
                PrintColoredLine(lineBuilder.ToString(), startX + (playGround.Dimension.X - lineBuilder.Length),
                    startY + y, currentColor);
            }
        }
        
        Console.ResetColor();
        Console.SetCursorPosition(actualX, actualY);
    }
    
    private static void PrintColoredLine(string line, int x, int y, ConsoleColor color)
    {
        Console.SetCursorPosition(x, y);
        Console.BackgroundColor = color;
        Console.Write(line);
        Console.ResetColor();
    }
    
    public static void SetConsoleSize(Vector dimension)
    {
        var width = dimension.X;
        var height = dimension.Y;
        
        if (width > 0 && width <= Console.LargestWindowWidth && height > 0 && height <= Console.LargestWindowHeight)
        {
            Console.SetWindowSize(width, height); 
            Console.SetBufferSize(width, height); 
        }
        else
        {
            Console.WriteLine("Fehler: Die angegebene Größe überschreitet die maximale Fenstergröße.");
        }
    }
}