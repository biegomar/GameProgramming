using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CellularAutomata;
using CellularAutomata.Cells;

namespace AvaloniaGameOfLive;

public static class GameOfLifeInitializer
{
    public static void Randomize(PlayGround playground, int maxDegreeOfParallelism, double aliveProbability = 0.2)
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
                    playground.SetCell(new Vector(x, y), state 
                        ? new Cell(CellType.Solid, CellBrightness.Solid)
                        : new Cell(CellType.Empty, CellBrightness.Empty));
                }
            }
        });
        
    }

    public static void PrepareFreestyle(PlayGround playground)
    {
        AddBlinker(playground, new Vector(4, 4));
        AddBlinker(playground, new Vector(8, 4));
        AddGlider(playground, new Vector(4, 8));
    }

    public static void AddCheckerboard(PlayGround playground)
    {
        for (int y = 0; y < playground.Dimension.Y; y++)
        {
            AddCheckerLine(playground, y);
        }
    }
    
    private static void AddCheckerLine(PlayGround playground, int row)
    {
        for (int x = 0; x < playground.Dimension.X; x++)
        {
            playground.SetCell(new Vector(x, row), int.IsEvenInteger(x) && int.IsEvenInteger(row) || int.IsOddInteger(x) && int.IsOddInteger(row) 
                ? new Cell(CellType.Solid, CellBrightness.Solid)
                : new Cell(CellType.Empty, CellBrightness.Empty));
        } 
    }

    public static void AddSingleCell(PlayGround playground, Vector position)
    {
        playground.SetCell(new Vector(position.X, position.Y), new Cell(CellType.Solid, CellBrightness.Solid));
    }
    
    // **Muster 1: Blinker (kleiner Oszillator)**
    public static void AddBlinker(PlayGround playground, Vector startPosition)
    { 
        playground.SetCell(new Vector(startPosition.X, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
    }

    // **Muster 2: Glider (bewegliches Muster)**
    public static void AddGlider(PlayGround playground, Vector startPosition)
    {
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y + 2), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y + 2), new Cell(CellType.Solid, CellBrightness.Solid));
    }

    // **Muster 3: Toad (größerer Oszillator)**
    public static void AddToad(PlayGround playground, Vector startPosition)
    {
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 3, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
    }

    // **Muster 4: Block (stabiler Zustand)**
    public static void AddBlock(PlayGround playground, Vector startPosition)
    {
        playground.SetCell(new Vector(startPosition.X, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
    }

    // **Muster 5: Beacon (kleiner oszillierender Zustand)**
    public static void AddBeacon(PlayGround playground, Vector startPosition)
    {
        // Oberer linker Block
        playground.SetCell(new Vector(startPosition.X, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));

        // Unterer rechter Block
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y + 2), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 3, startPosition.Y + 2), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y + 3), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 3, startPosition.Y + 3), new Cell(CellType.Solid, CellBrightness.Solid));
    }
}