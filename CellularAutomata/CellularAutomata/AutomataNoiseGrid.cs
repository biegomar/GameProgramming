using System.Collections.Concurrent;

namespace CellularAutomata;

public class AutomataNoiseGrid(Vector dimension)
{
    private PlayGroundArray nextGenerationPlayGround = new(dimension);

    public PlayGroundArray NextGenerationParallel(PlayGroundArray initialPlayGround, IRuleSet ruleSet, int maxDegreeOfParallelism)
    {
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = Math.Min(maxDegreeOfParallelism, Environment.ProcessorCount)
        };

        var ground = initialPlayGround;
        
        var xPartitioner = Partitioner.Create(0, initialPlayGround.Dimension.X);
        
        Parallel.ForEach(xPartitioner, parallelOptions, range =>
        {
            for (var column = range.Item1; column < range.Item2; column++) 
            {
                for (var row = 0; row < ground.Dimension.Y; row++)
                {
                    nextGenerationPlayGround[new Vector(column, row)] = ruleSet.ApplyRules(ground, new Vector(column, row));
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