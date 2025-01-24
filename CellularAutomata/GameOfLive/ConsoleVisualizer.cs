using System.Text;
using CellularAutomata;

namespace GameOfLive;

public static class ConsoleVisualizer
{
    static int counter = 0;
    public static void Render<T>(PlayGround<T> playGround, Func<T, char> stateToChar)
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
                var positionToCheck = new Vector(x, y, 0);
                lineBuilder.Append(stateToChar(playGround[positionToCheck]));
            }
            
            Console.SetCursorPosition(startX, startY + y);
            Console.Write(lineBuilder.ToString());
        }
        
        Console.SetCursorPosition(actualX, actualY);
    }
    
    public static void SetConsoleSize(Vector dimension)
    {
        var width = (int)dimension.X;
        var height = (int)dimension.Y;
        
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