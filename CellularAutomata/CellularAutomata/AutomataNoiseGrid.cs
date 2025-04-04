using System.Collections.Concurrent;

namespace CellularAutomata;

public class AutomataNoiseGrid(Vector dimension)
{
    private PlayGround nextGenerationPlayGround = new(dimension);

    public PlayGround NextGenerationParallel(PlayGround initialPlayGround, IRuleSet ruleSet, int maxDegreeOfParallelism)
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
    
    private static void Swap(ref PlayGround instanceOne, ref PlayGround instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }
}