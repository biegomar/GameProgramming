using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using CellularAutomata;
using CellularAutomata.Cells;
using CellularAutomata.Interfaces;

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

    public static void AddCheckerboard(IPlayGround playground)
    {
        for (int y = 0; y < playground.Dimension.Y; y++)
        {
            AddCheckerLine(playground, y);
        }
    }
    
    private static void AddCheckerLine(IPlayGround playground, int row)
    {
        for (int x = 0; x < playground.Dimension.X; x++)
        {
            playground.SetCell(new Vector(x, row), int.IsEvenInteger(x) && int.IsEvenInteger(row) || int.IsOddInteger(x) && int.IsOddInteger(row) 
                ? new Cell(CellType.Solid, CellBrightness.Solid)
                : new Cell(CellType.Empty, CellBrightness.Empty));
        } 
    }

    public static void AddSingleLineWithCellOnEveryXColumn(IPlayGround playground, int distance, int row)
    {
        for (int x = 0; x < playground.Dimension.X; x++)
        {
            playground[new Vector(x, row)] = x % distance == 0 ? CellBrightness.Solid : CellBrightness.Empty;
        } 
    }

    public static void AddSingleColumnWithCellOnEveryYRow(IPlayGround playground, int distance, int column)
    {
        for (int y = 0; y < playground.Dimension.Y; y++)
        {
            playground[new Vector(column, y)] = y % distance == 0 ? CellBrightness.Solid : CellBrightness.Empty;
        } 
    }

    public static void AddSingleCell(IPlayGround playground, Vector position)
    {
        playground[position] = CellBrightness.Solid;
    }
    
    // **Muster 1: Blinker (kleiner Oszillator)**
    public static void AddBlinker(IPlayGround playground, Vector startPosition)
    { 
        playground[new Vector(startPosition.X, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = CellBrightness.Solid;
    }

    // **Muster 2: Glider (bewegliches Muster)**
    public static void AddGlider(IPlayGround playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = CellBrightness.Solid;        // Zelle oben rechts
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = CellBrightness.Solid;        // Zelle Mitte links
        playground[new Vector(startPosition.X + 2, startPosition.Y + 1)] = CellBrightness.Solid;    // Zelle Mitte rechts
        playground[new Vector(startPosition.X + 1, startPosition.Y + 2)] = CellBrightness.Solid;    // Zelle Mitte unten
        playground[new Vector(startPosition.X + 2, startPosition.Y + 2)] = CellBrightness.Solid;    // Zelle unten rechts 
    }

    // **Muster 3: Toad (größerer Oszillator)**
    public static void AddToad(IPlayGround playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 3, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 2, startPosition.Y + 1)] = CellBrightness.Solid; 
    }

    // **Muster 4: Block (stabiler Zustand)**
    public static void AddBlock(IPlayGround playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1)] = CellBrightness.Solid; 
    }

    // **Muster 5: Beacon (kleiner oszillierender Zustand)**
    public static void AddBeacon(IPlayGround playground, Vector startPosition)
    {
        // Oberer linker Block
        playground[new Vector(startPosition.X, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1)] = CellBrightness.Solid;

        // Unterer rechter Block
        playground[new Vector(startPosition.X + 2, startPosition.Y + 2)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 3, startPosition.Y + 2)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 2, startPosition.Y + 3)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 3, startPosition.Y + 3)] = CellBrightness.Solid;   
    }
}