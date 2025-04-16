using System.Runtime.CompilerServices;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.Wolfram;

public sealed class AutomataWolfram(SimplePlayGround initialPlayGround)
{
    private SimplePlayGround nextGenerationPlayGround = new(initialPlayGround);
    
    public SimplePlayGround NextGenerationParallel(SimplePlayGround initialPlayGround, WolframRuleSet ruleSet, int row, int maxDegreeOfParallelism)
    {
        if (row >= initialPlayGround.Dimension.Y - 1 || row < 0)
        {
            return initialPlayGround;
        }
        
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = Math.Min(maxDegreeOfParallelism, Environment.ProcessorCount)
        };
        
        var ground = initialPlayGround;

        // for (var column = 0; column < ground.Dimension.X; column++)
        // {
        //     var state = ruleSet.ApplyRules(ground, new Vector(column, row));
        //     nextGenerationPlayGround.SetState(new Vector(column, row + 1), state);
        // }
        
        Parallel.For(0, ground.Dimension.X, parallelOptions, column =>
        {
            nextGenerationPlayGround.SetState(new Vector(column, row + 1), ruleSet.ApplyRules(ground, new Vector(column, row)));
        });
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround); 
        
        return initialPlayGround;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Swap(ref SimplePlayGround instanceOne, ref SimplePlayGround instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }
}