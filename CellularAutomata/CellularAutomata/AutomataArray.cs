using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace CellularAutomata;

public sealed class AutomataArray
{
    private PlayGroundArray? nextGenerationPlayGround;
    
    private void InitNextGenerationPlayGround(Vector dimension)
    {
        if (nextGenerationPlayGround == null || nextGenerationPlayGround.Dimension != dimension)
        {
            nextGenerationPlayGround = new PlayGroundArray(dimension);
        }
    }
    
    public AutomataArray(Vector dimension)
    {
        this.InitNextGenerationPlayGround(dimension);
    }
    
    public PlayGroundArray NextGeneration(PlayGroundArray initialPlayGround, IRuleSet ruleSet, bool isSpawn)
    {
        for (var column = 0; column < initialPlayGround.Dimension.X; column++)
        {
            for (var row = 0; row < initialPlayGround.Dimension.Y; row++)
            {
                nextGenerationPlayGround![(column, row)] = ruleSet.ApplyRules(initialPlayGround, (column, row));    
            }
        }
        
        nextGenerationPlayGround = (PlayGroundArray)ruleSet.ApplySpawnRules(nextGenerationPlayGround!, isSpawn);
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    public PlayGroundArray NextGenerationParallel(PlayGroundArray initialPlayGround, IRuleSet ruleSet, bool isSpawn, int maxDegreeOfParallelism)
    {
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = Math.Min(maxDegreeOfParallelism, Environment.ProcessorCount)
        };

        if (nextGenerationPlayGround != null)
        {
            // var ground = initialPlayGround;
            // Parallel.For(0, initialPlayGround.Dimension.X, parallelOptions, x =>
            // {
            //     for (var y = 0; y < ground.Dimension.Y; y++)
            //     {
            //         nextGenerationPlayGround[(x, y)] = ruleSet.ApplyRules(ground, (x, y));
            //     }
            // });
            
            var ground = initialPlayGround;
            
            var xPartitioner = Partitioner.Create(0, initialPlayGround.Dimension.X);

            Parallel.ForEach(xPartitioner, parallelOptions, range =>
            {
                for (var x = range.Item1; x < range.Item2; x++) 
                {
                    for (var y = 0; y < ground.Dimension.Y; y++)
                    {
                        nextGenerationPlayGround[(x, y)] = ruleSet.ApplyRules(ground, (x, y));
                    }
                }
            });

            
            nextGenerationPlayGround = (PlayGroundArray)ruleSet.ApplySpawnRules(nextGenerationPlayGround, isSpawn);
        
            Swap(ref initialPlayGround, ref nextGenerationPlayGround);    
        }
        
        return initialPlayGround;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Swap(ref PlayGroundArray instanceOne, ref PlayGroundArray instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }
}