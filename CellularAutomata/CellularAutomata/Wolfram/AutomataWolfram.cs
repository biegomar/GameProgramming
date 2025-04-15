using CellularAutomata.Interfaces;

namespace CellularAutomata.Wolfram;

public sealed class AutomataWolfram
{
    public PlayGround NextGenerationParallel(PlayGround initialPlayGround, IRuleSet ruleSet, int row, int maxDegreeOfParallelism)
    {
        if (row >= initialPlayGround.Dimension.Y - 1 || row < 0)
        {
            return initialPlayGround;
        }
        
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = Math.Min(maxDegreeOfParallelism, Environment.ProcessorCount)
        };
        
        Parallel.For(0, initialPlayGround.Dimension.X, parallelOptions, column =>
        {
            var result = ruleSet.ApplyMaterialRules(initialPlayGround, new Vector(column, row));
            if (result.HasValue)
            {
                initialPlayGround.SetCell(new Vector(column, row + 1), result.Value.Source.Body);
            }
        });
        
        return initialPlayGround;
    }
}