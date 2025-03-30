namespace CellularAutomata;

public sealed class AutomataWolfram
{
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
            initialPlayGround[new Vector(x, row + 1)] = ruleSet.ApplyRules(initialPlayGround, new Vector(x, row)); 
        });
        
        return initialPlayGround;
    }
}