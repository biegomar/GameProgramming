using System.Runtime.CompilerServices;

namespace CellularAutomata;

public static class Automata<T>
{
    private static PlayGround<T>? nextGenerationPlayGround;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void InitNextGenerationPlayGround(PlayGround<T> initialPlayGround)
    {
        if (nextGenerationPlayGround == null || nextGenerationPlayGround.Dimension != initialPlayGround.Dimension)
        {
            nextGenerationPlayGround = new PlayGround<T>(initialPlayGround.Dimension);
        }
    }
    
    public static PlayGround<T> NextGeneration(PlayGround<T> initialPlayGround, IRuleSet<T> ruleSet, bool isSpawn)
    {
        InitNextGenerationPlayGround(initialPlayGround);
        
        foreach (var cell in initialPlayGround.Cells)
        {
            nextGenerationPlayGround![cell.Key] = ruleSet.ApplyRules(initialPlayGround, cell.Key); 
        }
        
        nextGenerationPlayGround = (PlayGround<T>)ruleSet.ApplySpawnRules(nextGenerationPlayGround!, isSpawn);
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }

    public static PlayGround<T> NextGenerationParallel(PlayGround<T> initialPlayGround, IRuleSet<T> ruleSet, bool isSpawn, int maxDegreeOfParallelism)
    {
        InitNextGenerationPlayGround(initialPlayGround);

        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism
        };
        
        Parallel.ForEach(initialPlayGround.Cells, parallelOptions, cell =>
        {
            nextGenerationPlayGround![cell.Key] = ruleSet.ApplyRules(initialPlayGround, cell.Key);
        });
        
        nextGenerationPlayGround = (PlayGround<T>)ruleSet.ApplySpawnRules(nextGenerationPlayGround!, isSpawn);
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    public static PlayGround<T> NextGenerationForLoop(PlayGround<T> initialPlayGround, IRuleSet<T> ruleSet, bool isSpawn)
    {
        var newPlayGround = new PlayGround<T>(initialPlayGround.Dimension);
        var dimensionX = initialPlayGround.Dimension.X;
        var dimensionY = initialPlayGround.Dimension.Y;
        var position = new Vector(0,0);
        
        for (var x = 0; x < dimensionX; x++)
        {
            position.X = x;
            for (var y = 0; y < dimensionY; y++)
            {
                position.Y = y;
                newPlayGround[position] = ruleSet.ApplyRules(initialPlayGround, position);
            }
        }
        
        var resultPlayGround = (PlayGround<T>)ruleSet.ApplySpawnRules(newPlayGround, isSpawn);
        
        return resultPlayGround;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void Swap(ref PlayGround<T> instanceOne, ref PlayGround<T> instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }
}