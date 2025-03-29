using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Linq;

namespace CellularAutomata;

public sealed class AutomataArray(Vector dimension)
{
    private PlayGroundArray nextGenerationPlayGround = new(dimension);

    public PlayGroundArray NextGeneration(PlayGroundArray initialPlayGround, IRuleSet ruleSet, bool isSpawn, Vector spawnPosition, Vector brushSize)
    {
        for (var column = 0; column < initialPlayGround.Dimension.X; column++)
        {
            for (var row = 0; row < initialPlayGround.Dimension.Y; row++)
            {
                var posVector = new Vector(column, row);
                nextGenerationPlayGround[posVector] = ruleSet.ApplyRules(initialPlayGround, posVector);    
            }
        }

        if (isSpawn)
        {
            nextGenerationPlayGround = ApplySpawnRules(nextGenerationPlayGround, ruleSet, spawnPosition, brushSize);    
        }
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    public PlayGroundArray NextGenerationParallel(PlayGroundArray initialPlayGround, IRuleSet ruleSet, bool isSpawn, Vector spawnPosition, Vector brushSize, int maxDegreeOfParallelism)
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
                    var posVector = new Vector(column, row);
                    nextGenerationPlayGround[posVector] = ruleSet.ApplyRules(ground, posVector);
                }
            }
        });

        if (isSpawn)
        {
            nextGenerationPlayGround = ApplySpawnRules(nextGenerationPlayGround, ruleSet, spawnPosition, brushSize);   
        }
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround); 
        
        return initialPlayGround;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PlayGroundArray ApplySpawnRules(PlayGroundArray playGround, IRuleSet ruleSet, Vector spawnPosition, Vector brushSize, double probability = 1)
    {
        return (PlayGroundArray)ruleSet.ApplySpawnRules(playGround, spawnPosition, brushSize, probability); 
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Swap(ref PlayGroundArray instanceOne, ref PlayGroundArray instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }
}