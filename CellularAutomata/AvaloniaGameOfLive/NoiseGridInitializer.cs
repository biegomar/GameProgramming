using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CellularAutomata;
using CellularAutomata.Cells;
using CellularAutomata.Interfaces;

namespace AvaloniaGameOfLive;

public static class NoiseGridInitializer
{
    public static void Randomize(PlayGround playground, int maxDegreeOfParallelism, double density = 0.65)
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
                    var state = random.NextDouble() < density;
                    playground.SetCell(new Vector(x, y), state 
                        ? new Cell(CellType.Empty, CellBrightness.Empty)
                        : new Cell(CellType.Solid, CellBrightness.Solid));
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
}