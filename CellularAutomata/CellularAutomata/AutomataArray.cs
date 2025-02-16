namespace CellularAutomata;

public static class AutomataArray<T>
{
    private static PlayGroundArray<T>? backupPlayGround;
    private static PlayGroundArray<T>? nextGenerationPlayGround;

    private static void InitBackupPlayGround(PlayGroundArray<T> initialPlayGround)
    {
        if (backupPlayGround == null || backupPlayGround.Dimension != initialPlayGround.Dimension)
        {
            backupPlayGround = new PlayGroundArray<T>(initialPlayGround.Dimension);
        }
    }

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
        InitBackupPlayGround(initialPlayGround);
        
        foreach (var cell in initialPlayGround.Cells)
        {
            nextGenerationPlayGround[cell.Position] = ruleSet.ApplyRules(initialPlayGround, cell.Position); 
        }
        
        nextGenerationPlayGround = (PlayGroundArray<T>)ruleSet.ApplySpawnRules(nextGenerationPlayGround, isSpawn);
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    public static PlayGroundArray<T> NextGenerationParallel(PlayGroundArray<T> initialPlayGround, IRuleSet<T> ruleSet, bool isSpawn)
    {
        InitNextGenerationPlayGround(initialPlayGround);
        InitBackupPlayGround(initialPlayGround);

        Parallel.ForEach(initialPlayGround.Cells.Cast<Cell<T>>(), cell =>
        {
            nextGenerationPlayGround[cell.Position] = ruleSet.ApplyRules(initialPlayGround, cell.Position);
        });
        
        nextGenerationPlayGround = (PlayGroundArray<T>)ruleSet.ApplySpawnRules(nextGenerationPlayGround, isSpawn);
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    static void Swap(ref PlayGroundArray<T> instanceOne, ref PlayGroundArray<T> instanceTwo)
    { 
        InitBackupPlayGround(instanceOne);
        backupPlayGround = instanceOne;
        instanceOne = instanceTwo;
        instanceTwo = backupPlayGround;
    }
}