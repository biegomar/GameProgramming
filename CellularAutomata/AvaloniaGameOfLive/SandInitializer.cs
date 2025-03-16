using System;
using System.Threading.Tasks;
using CellularAutomata;

namespace AvaloniaGameOfLive;

public static class SandInitializer
{
    public static void Randomize(IPlayGround<SandCellState> playground, double aliveProbability = 0.2)
    {
        var random = new Random();

        if (playground is PlayGround<SandCellState> playGroundBool)
        {
            Parallel.ForEach(playGroundBool.Cells, cell =>
            {
                var state = random.NextDouble() < aliveProbability;
                playground[cell.Key] = state ? GetRandomSandCellState() : SandCellState.Empty;
            });
        }
        else if (playground is PlayGroundArray<SandCellState> playGroundArrayBool)
        {
            Parallel.ForEach(playGroundArrayBool.Cells, cell =>
            {
                var state = random.NextDouble() < aliveProbability;
                playGroundArrayBool[(cell.X, cell.Y)] = state ? GetRandomSandCellState() : SandCellState.Empty;
            });
        }
    }
    
    public static void AddSandCellStateToCell(IPlayGround<SandCellState> playground, Vector position, SandCellState state)
    {
        playground[position] = state; 
    }
    
    public static void GenerateSandHourglass(IPlayGround<SandCellState> playground)
    {
        var dimension = playground.Dimension;
        var width = dimension.X;
        var height = dimension.Y;

        int midX = width / 2; // Mitte der Breite
        int midY = height / 2; // Mitte der Höhe

        // Sanduhr von oben nach unten aufbauen
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (IsOutline(x, y, width, height) && !IsConnection(x, y, midX, midY))
                {
                    playground[new Vector(x, y)] = SandCellState.Solid;
                }
                else if (IsTopSand(x, y, midX, midY))
                {
                    playground[new Vector(x, y)] = GetRandomSandCellState();
                }
                else if (IsConnection(x, y, midX, midY))
                {
                    playground[new Vector(x, y)] = GetRandomSandCellState();
                }
                else if (IsBottomEmpty(x, y, midX, midY))
                {
                    playground[new Vector(x, y)] = SandCellState.Empty;
                }
            }
        }
    }
    
    // Überprüft, ob die Zelle Teil des äußeren Rahmens ist
    private static bool IsOutline(int x, int y, int width, int height)
    {
        int midX = width / 2;
        int midY = height / 2;

        // Obere Hälfte (Sanduhr-Umrandung für oberen Kolben)
        if (y <= midY && Math.Abs(x - midX) == (midY - y))
        {
            return true; // Rand für Top (triangular)
        }

        // Untere Hälfte (Sanduhr-Umrandung für unteren Kolben)
        if (y >= midY && Math.Abs(x - midX) == (y - midY))
        {
            return true; // Rand für Bottom (inverted triangular)
        }

        // Kein Rand
        return false;
    }


    // Überprüft, ob die Zelle zum oberen (gefüllten) Sand passt
    private static bool IsTopSand(int x, int y, int midX, int midY)
    {
        return y < midY && Math.Abs(x - midX) <= (midY - y - 1);
    }

    // Überprüft, ob die Zelle die Verbindung (einen Punkt breit) zwischen den Kolben ist
    private static bool IsConnection(int x, int y, int midX, int midY)
    {
        // Genau die Mitte der Sanduhr (1 Zelle)
        return y == midY && x == midX;

    }

    // Überprüft, ob die Zelle im unteren Kolben (leer) liegt
    private static bool IsBottomEmpty(int x, int y, int midX, int midY)
    {
        return y > midY && Math.Abs(x - midX) <= (y - midY - 1);
    }
    
    private static SandCellState GetRandomSandCellState()
    {
        var random = new Random();
        var randomValue = random.Next(2, 6);

        return (SandCellState)randomValue;

    }
}