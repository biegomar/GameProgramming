using System.Collections.Concurrent;

namespace CellularAutomata;

public class AutomataNoiseGrid(Vector dimension)
{
    private PlayGroundArray nextGenerationPlayGround = new(dimension);

    public PlayGroundArray NextGenerationParallel(PlayGroundArray initialPlayGround, IRuleSet ruleSet, int row, int maxDegreeOfParallelism)
    {
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = Math.Min(maxDegreeOfParallelism, Environment.ProcessorCount)
        };

        var ground = initialPlayGround;
        
        var xPartitioner = Partitioner.Create(0, initialPlayGround.Dimension.X);
        
        Parallel.ForEach(xPartitioner, parallelOptions, range =>
        {
            for (var x = range.Item1; x < range.Item2; x++) 
            {
                for (var y = 0; y < ground.Dimension.Y; y++)
                {
                    nextGenerationPlayGround[(x, y)] = ruleSet.ApplyRules(ground, new Vector(x, y));
                }
            }
        });
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround); 
        
        return initialPlayGround;
    } 
    
    private static void Swap(ref PlayGroundArray instanceOne, ref PlayGroundArray instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }
}