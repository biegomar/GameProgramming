using System.Runtime.CompilerServices;

namespace CellularAutomata;

public static class AutomataArray<T>
{
    private static PlayGroundArray<T>? nextGenerationPlayGround;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void InitNextGenerationPlayGround(PlayGroundArray<T> initialPlayGround)
    {
        if (nextGenerationPlayGround == null || nextGenerationPlayGround.Dimension != initialPlayGround.Dimension)
        {
            nextGenerationPlayGround = new PlayGroundArray<T>(initialPlayGround.Dimension);
        }
    }
    
    public static PlayGroundArray<T> NextGeneration(PlayGroundArray<T> initialPlayGround, IRuleSet<T> ruleSet, bool isSpawn)
    {
        InitNextGenerationPlayGround(initialPlayGround);
        
        foreach (var cell in initialPlayGround.Cells)
        {
            nextGenerationPlayGround![(cell.X, cell.Y)] = ruleSet.ApplyRules(initialPlayGround, (cell.X, cell.Y)); 
        }
        
        nextGenerationPlayGround = (PlayGroundArray<T>)ruleSet.ApplySpawnRules(nextGenerationPlayGround!, isSpawn);
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    public static PlayGroundArray<T> NextGenerationParallel(PlayGroundArray<T> initialPlayGround, IRuleSet<T> ruleSet, bool isSpawn, int maxDegreeOfParallelism)
    {
        InitNextGenerationPlayGround(initialPlayGround);

        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism
        };

        Parallel.ForEach(initialPlayGround.Cells.Cast<Cell<T>>(), parallelOptions, cell =>
        {
            nextGenerationPlayGround![(cell.X, cell.Y)] = ruleSet.ApplyRules(initialPlayGround, (cell.X, cell.Y));
        });
        
        nextGenerationPlayGround = (PlayGroundArray<T>)ruleSet.ApplySpawnRules(nextGenerationPlayGround!, isSpawn);
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void Swap(ref PlayGroundArray<T> instanceOne, ref PlayGroundArray<T> instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }
}