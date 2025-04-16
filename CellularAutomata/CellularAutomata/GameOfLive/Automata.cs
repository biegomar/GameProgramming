using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using CellularAutomata.Interfaces;

namespace CellularAutomata.GameOfLive;

public sealed class Automata(Vector dimension)
{
    private PlayGround nextGenerationPlayGround = new(dimension);

    public PlayGround NextGeneration(PlayGround initialPlayGround, GameOfLifeRuleSet ruleSet, bool isSpawn, Vector spawnPosition, Vector brushSize)
    {
        InitializeNextGenerationPlayGround(initialPlayGround, 16);
        for (var row = 0; row < initialPlayGround.Dimension.Y; row++)
        {
            for (var column = 0; column < initialPlayGround.Dimension.X; column++)
            {
                var result = ruleSet.ApplyMaterialRules(initialPlayGround, new Vector(column, row));
                if (result.HasValue)
                {
                    nextGenerationPlayGround.SetCell(new Vector(column, row), result.Value.Source.Body);
                }
            }
        }

        if (isSpawn)
        {
            nextGenerationPlayGround = ApplySpawnRules(nextGenerationPlayGround, ruleSet, spawnPosition, brushSize);    
        }
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    public PlayGround NextGenerationParallel(PlayGround initialPlayGround, GameOfLifeRuleSet ruleSet, bool isSpawn, Vector spawnPosition, Vector brushSize, int maxDegreeOfParallelism)
    {
        InitializeNextGenerationPlayGround(initialPlayGround, maxDegreeOfParallelism);
        
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
                    var result = ruleSet.ApplyMaterialRules(ground, new Vector(column, row));
                    if (result.HasValue)
                    {
                        nextGenerationPlayGround.SetCell(new Vector(column, row), result.Value.Source.Body);
                    }
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
    public PlayGround ApplySpawnRules(PlayGround playGround, GameOfLifeRuleSet ruleSet, Vector spawnPosition, Vector brushSize, double probability = 1)
    {
        return (PlayGround)ruleSet.ApplySpawnRules(playGround, spawnPosition, brushSize, probability); 
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Swap(ref PlayGround instanceOne, ref PlayGround instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }
    
    private void InitializeNextGenerationPlayGround(PlayGround playGround, int maxDegreeOfParallelism)
    {
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = Math.Min(maxDegreeOfParallelism, Environment.ProcessorCount)
        };
        
        
        var yPartitioner = Partitioner.Create(0, playGround.Dimension.Y);
        
        Parallel.ForEach(yPartitioner, parallelOptions, (range, loopState) =>
        {
            for (var row = range.Item1; row < range.Item2; row++) 
            {
                for (var column = 0; column < playGround.Dimension.X; column++)
                {
                    nextGenerationPlayGround.SetCell(new Vector(column, row), playGround.GetCell(new Vector(column, row)) with {});
                }
            }
        });
    }
}