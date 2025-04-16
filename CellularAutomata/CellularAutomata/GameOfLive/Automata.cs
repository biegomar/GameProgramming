using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.GameOfLive;

public sealed class Automata(Vector dimension)
{
    private SimplePlayGround nextGenerationPlayGround = new(dimension);

    public SimplePlayGround NextGeneration(SimplePlayGround initialPlayGround, GameOfLifeRuleSet ruleSet, bool isSpawn, Vector spawnPosition, Vector brushSize)
    {
        for (var row = 0; row < initialPlayGround.Dimension.Y; row++)
        {
            for (var column = 0; column < initialPlayGround.Dimension.X; column++)
            {
                var state = ruleSet.ApplyRules(initialPlayGround, new Vector(column, row));
                nextGenerationPlayGround.SetState(new Vector(column, row), state);
            }
        }

        if (isSpawn)
        {
            nextGenerationPlayGround = ApplySpawnRules(nextGenerationPlayGround, ruleSet, spawnPosition, brushSize);    
        }
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    public SimplePlayGround NextGenerationParallel(SimplePlayGround initialPlayGround, GameOfLifeRuleSet ruleSet, bool isSpawn, Vector spawnPosition, Vector brushSize, int maxDegreeOfParallelism)
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
                    var state = ruleSet.ApplyRules(ground, new Vector(column, row));
                    nextGenerationPlayGround.SetState(new Vector(column, row), state);
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
    public SimplePlayGround ApplySpawnRules(SimplePlayGround playGround, GameOfLifeRuleSet ruleSet, Vector spawnPosition, Vector brushSize, double probability = 1)
    {
        return ruleSet.ApplySpawnRules(playGround, spawnPosition, brushSize, probability); 
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Swap(ref SimplePlayGround instanceOne, ref SimplePlayGround instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }
}