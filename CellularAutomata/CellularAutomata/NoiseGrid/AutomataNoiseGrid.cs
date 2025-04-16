using System.Collections.Concurrent;

namespace CellularAutomata.NoiseGrid;

public class AutomataNoiseGrid(Vector dimension)
{
    private PlayGround nextGenerationPlayGround = new(dimension);

    public PlayGround NextGenerationParallel(PlayGround initialPlayGround, NoiseGridRuleSet ruleSet, int maxDegreeOfParallelism)
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
                    var result = ruleSet.ApplyMaterialRules(ground, new Vector(column, row));
                    if (result.HasValue)
                    {
                        nextGenerationPlayGround.SetCell(new Vector(column, row), result.Value.Source.Body);
                    }
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