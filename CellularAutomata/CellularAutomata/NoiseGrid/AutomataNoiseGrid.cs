using System.Collections.Concurrent;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.NoiseGrid;

public class AutomataNoiseGrid(Vector dimension)
{
    private SimplePlayGround nextGenerationPlayGround = new(dimension);

    public SimplePlayGround NextGenerationParallel(SimplePlayGround initialPlayGround, NoiseGridRuleSet ruleSet, int maxDegreeOfParallelism)
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
                    var state = ruleSet.ApplyRules(ground, new Vector(column, row));
                    nextGenerationPlayGround.SetState(new Vector(column, row), state);
                }
            }
        });
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround); 
        
        return initialPlayGround;
    } 
    
    private static void Swap(ref SimplePlayGround instanceOne, ref SimplePlayGround instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }
}