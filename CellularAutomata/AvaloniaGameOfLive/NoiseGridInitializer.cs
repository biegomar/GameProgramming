using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CellularAutomata;

namespace AvaloniaGameOfLive;

public static class NoiseGridInitializer
{
    public static void Randomize(IPlayGround playground, int maxDegreeOfParallelism, double density = 0.65)
    {
        var random = new Random();

        if (playground is PlayGround playGroundBool)
        {
            Parallel.ForEach(playGroundBool.Cells, cell =>
            {
                var state = random.NextDouble() > density;
                playground[cell.Key] = state ? CellState.Empty : CellState.Solid;
            });
        }
        else if (playground is PlayGroundArray playGroundArrayBool)
        {
            var parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Math.Min(maxDegreeOfParallelism, Environment.ProcessorCount)
            };
            
            var xPartitioner = Partitioner.Create(0, playGroundArrayBool.Dimension.X);
            Parallel.ForEach(xPartitioner, parallelOptions, range =>
            {
                for (var x = range.Item1; x < range.Item2; x++) 
                {
                    for (var y = 0; y < playGroundArrayBool.Dimension.Y; y++)
                    {
                        var state = random.NextDouble() > density;
                        playGroundArrayBool[(x, y)] = state ? CellState.Empty : CellState.Solid;
                    }
                }
            });
        }
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
            playground[(x, row)] = int.IsEvenInteger(x) && int.IsEvenInteger(row) || int.IsOddInteger(x) && int.IsOddInteger(row) ? CellState.Solid : CellState.Empty;
        } 
    }
}