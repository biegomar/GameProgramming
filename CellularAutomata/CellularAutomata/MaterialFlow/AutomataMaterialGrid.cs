using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow;

public sealed class AutomataMaterialGrid(Vector dimension)
{
    private PlayGround nextGenerationPlayGround = new(dimension);
    
    public PlayGround NextGeneration(PlayGround initialPlayGround, MaterialRuleSet ruleSet, int maxDegreeOfParallelism)
    {
        ResetNextGenerationPlayGround(initialPlayGround, maxDegreeOfParallelism);
        
        var ground = initialPlayGround;
        
        for (var row = 0; row < initialPlayGround.Dimension.Y; row++)
        {
            for (var column = 0; column < initialPlayGround.Dimension.X; column++)
            {
                var result = ruleSet.ApplyRules(ground, new Vector(column, row));
                if (result.HasValue)
                {
                    nextGenerationPlayGround.SetCell(new Vector(column, row), result.Value.Source.Body);
                    if (result.Value.Destination.HasValue)
                    {
                        nextGenerationPlayGround.SetCell(result.Value.Destination.Value.Position, result.Value.Destination.Value.Body);
                    }
                }
            }
        }
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    public PlayGround NextGenerationParallel(PlayGround initialPlayGround, MaterialRuleSet ruleSet, int maxDegreeOfParallelism)
    {
        ResetNextGenerationPlayGround(initialPlayGround, maxDegreeOfParallelism);
        
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
                     var result = ruleSet.ApplyRules(ground, new Vector(column, row));
                     if (result.HasValue)
                     {
                         nextGenerationPlayGround.SetCell(result.Value.Source.Position, result.Value.Source.Body);
                         if (result.Value.Destination.HasValue)
                         {
                             nextGenerationPlayGround.SetCell(result.Value.Destination.Value.Position, result.Value.Destination.Value.Body);    
                         }   
                     }
                }
            }
        });
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround); 
        
        return initialPlayGround;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PlayGround ApplySpawnRules(PlayGround playGround, MaterialRuleSet ruleSet, CellType type, Vector spawnPosition, Vector brushSize, double probability = 1)
    {
        return ruleSet.ApplySpawnRules(playGround, type, spawnPosition, brushSize, probability); 
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Swap(ref PlayGround instanceOne, ref PlayGround instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ResetNextGenerationPlayGround(PlayGround playGround, int maxDegreeOfParallelism)
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
                    nextGenerationPlayGround.SetCell(new Vector(column, row), playGround.GetCell(new Vector(column, row)));
                }
            }
        });
    }
}