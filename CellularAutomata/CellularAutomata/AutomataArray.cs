using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Linq;

namespace CellularAutomata;

public sealed class AutomataArray
{
    private PlayGroundArray nextGenerationPlayGround;
    
    public AutomataArray(Vector dimension)
    {
        nextGenerationPlayGround = new PlayGroundArray(dimension);
    }
    
    public PlayGroundArray NextGeneration(PlayGroundArray initialPlayGround, IRuleSet ruleSet, bool isSpawn, Vector spawnPosition)
    {
        for (var column = 0; column < initialPlayGround.Dimension.X; column++)
        {
            for (var row = 0; row < initialPlayGround.Dimension.Y; row++)
            {
                nextGenerationPlayGround[(column, row)] = ruleSet.ApplyRules(initialPlayGround, new Vector(column, row));    
            }
        }

        if (isSpawn)
        {
            nextGenerationPlayGround = (PlayGroundArray)ruleSet.ApplySpawnRules(nextGenerationPlayGround, spawnPosition);    
        }
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    public PlayGroundArray NextGenerationParallel(PlayGroundArray initialPlayGround, IRuleSet ruleSet, bool isSpawn, Vector spawnPosition, int maxDegreeOfParallelism)
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

        if (isSpawn)
        {
            nextGenerationPlayGround = (PlayGroundArray)ruleSet.ApplySpawnRules(nextGenerationPlayGround, spawnPosition);   
        }
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround); 
        
        return initialPlayGround;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Swap(ref PlayGroundArray instanceOne, ref PlayGroundArray instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }
}