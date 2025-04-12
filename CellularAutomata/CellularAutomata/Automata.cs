using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Linq;

namespace CellularAutomata;

public sealed class Automata(Vector dimension)
{
    private PlayGround nextGenerationPlayGround = new(dimension);

    public PlayGround NextGeneration(PlayGround initialPlayGround, IRuleSet ruleSet, bool isSpawn, Vector spawnPosition, Vector brushSize)
    {
        for (var row = 0; row < initialPlayGround.Dimension.Y; row++)
        {
            for (var column = 0; column < initialPlayGround.Dimension.X; column++)
            {
                nextGenerationPlayGround[new Vector(column, row)] = ruleSet.ApplyRules(initialPlayGround, new Vector(column, row));    
            }
        }

        if (isSpawn)
        {
            nextGenerationPlayGround = ApplySpawnRules(nextGenerationPlayGround, ruleSet, spawnPosition, brushSize);    
        }
        
        initialPlayGround.ResetMovedCells();
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    public PlayGround NextGenerationParallel(PlayGround initialPlayGround, IRuleSet ruleSet, bool isSpawn, Vector spawnPosition, Vector brushSize, int maxDegreeOfParallelism)
    {
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = Math.Min(maxDegreeOfParallelism, Environment.ProcessorCount)
        };

        var ground = initialPlayGround;
        
        var yPartitioner = Partitioner.Create(0, initialPlayGround.Dimension.Y);
        
        Parallel.ForEach(yPartitioner, parallelOptions, (range, loopState) =>
        {
            for (var row = range.Item1; row < range.Item2; row++) 
            {
                for (var column = 0; column < ground.Dimension.X; column++)
                {
                    nextGenerationPlayGround[new Vector(column, row)] = ruleSet.ApplyRules(ground, new Vector(column, row));
                }
            }
        });

        if (isSpawn)
        {
            nextGenerationPlayGround = ApplySpawnRules(nextGenerationPlayGround, ruleSet, spawnPosition, brushSize);   
        }
        
        initialPlayGround.ResetMovedCells();
        Swap(ref initialPlayGround, ref nextGenerationPlayGround); 
        
        return initialPlayGround;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PlayGround ApplySpawnRules(PlayGround playGround, IRuleSet ruleSet, Vector spawnPosition, Vector brushSize, double probability = 1)
    {
        return (PlayGround)ruleSet.ApplySpawnRules(playGround, spawnPosition, brushSize, probability); 
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Swap(ref PlayGround instanceOne, ref PlayGround instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }
}