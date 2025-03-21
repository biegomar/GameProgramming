namespace CellularAutomata;

public sealed class AutomataWolfram
{
    public PlayGroundArray NextGeneration(PlayGroundArray initialPlayGround, IRuleSet ruleSet, int row)
    {
        if (row >= initialPlayGround.Dimension.Y - 1)
        {
            return initialPlayGround;
        }
        
        for (var x = 0; x < initialPlayGround.Dimension.X; x++)
        {
            initialPlayGround[(x, row + 1)] = ruleSet.ApplyRules(initialPlayGround, (x, row));
        }
        
        return initialPlayGround;
    }
    
    public PlayGroundArray NextGenerationParallel(PlayGroundArray initialPlayGround, IRuleSet ruleSet, int row, int maxDegreeOfParallelism)
    {
        if (row >= initialPlayGround.Dimension.Y - 1)
        {
            return initialPlayGround;
        }
        
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = Math.Min(maxDegreeOfParallelism, Environment.ProcessorCount)
        };
        
        Parallel.For(0, initialPlayGround.Dimension.X, parallelOptions, x =>
        {
            initialPlayGround[(x, row + 1)] = ruleSet.ApplyRules(initialPlayGround, (x, row)); 
        });
        
        return initialPlayGround;
    }
}