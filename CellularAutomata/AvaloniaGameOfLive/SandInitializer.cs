using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CellularAutomata;
using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace AvaloniaGameOfLive;

public static class SandInitializer
{
    public static void Randomize(PlayGround playground, int maxDegreeOfParallelism, CellType cellType = CellType.Sand, double aliveProbability = 0.2)
    {
        var random = new Random();

        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = Math.Min(maxDegreeOfParallelism, Environment.ProcessorCount)
        };
            
        var xPartitioner = Partitioner.Create(0, playground.Dimension.X);
        Parallel.ForEach(xPartitioner, parallelOptions, range =>
        {
            for (var x = range.Item1; x < range.Item2; x++) 
            {
                for (var y = 0; y < playground.Dimension.Y; y++)
                {
                    var state = random.NextDouble() < aliveProbability;
                    playground.SetCell(new Vector(x, y), new Cell(state ? cellType : CellType.Empty, state ? ShadeProvider.GenerateColor(cellType) : CellColor.Empty));
                }
            }
        });
    }
    
    public static void GenerateSandHourglass(PlayGround playground)
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
                    playground.SetCell(new Vector(x, y), new Cell(CellType.Solid, CellColor.Solid));
                }
                else if (IsTopSand(x, y, midX, midY))
                {
                    playground.SetCell(new Vector(x, y), new Cell(CellType.Sand, ShadeProvider.GenerateColor(CellType.Sand)));
                }
                else if (IsConnection(x, y, midX, midY))
                {
                    playground.SetCell(new Vector(x, y), new Cell(CellType.Sand, ShadeProvider.GenerateColor(CellType.Sand)));
                }
                else if (IsBottomEmpty(x, y, midX, midY))
                {
                    playground.SetCell(new Vector(x, y), new Cell(CellType.Empty, CellColor.Empty));
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
}