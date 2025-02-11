namespace CellularAutomata;

public static class AutomataArray<T>
{
    public static PlayGroundArray<T> NextGeneration(PlayGroundArray<T> initialPlayGround, IRuleSet<T> ruleSet, bool isSpawn)
    {
        var newPlayGround = new PlayGroundArray<T>(initialPlayGround.Dimension);
        
        for (var x = 0; x < initialPlayGround.Dimension.X; x++)
        {
            for (var y = 0; y < initialPlayGround.Dimension.Y; y++)
            {
                for (var z = 0; z <= initialPlayGround.Dimension.Z; z++)
                {
                    newPlayGround[(x, y, z)] = ruleSet.ApplyRules(initialPlayGround, (x, y, z));
                    
                    if (initialPlayGround.Dimension.Z == 0) break;
                }
            }
        }

        
        var resultPlayGround = (PlayGroundArray<T>)ruleSet.ApplySpawnRules(newPlayGround, isSpawn);
        
        return resultPlayGround;
    }
    
    public static PlayGroundArray<T> NextGenerationParallel(PlayGroundArray<T> initialPlayGround, IRuleSet<T> ruleSet, bool isSpawn)
    {
        var newPlayGround = new PlayGroundArray<T>(initialPlayGround.Dimension);

        Parallel.For(0, (int)initialPlayGround.Dimension.X + 1 , x =>
        {
            for (var y = 0; y < initialPlayGround.Dimension.Y + 1; y++)
            {
                for (var z = 0; z <= initialPlayGround.Dimension.Z + 1; z++)
                {
                    newPlayGround[(x, y, z)] = ruleSet.ApplyRules(initialPlayGround, (x, y, z));
                    
                    if (initialPlayGround.Dimension.Z == 0) break;
                }
            }
        });

        var resultPlayGround = (PlayGroundArray<T>)ruleSet.ApplySpawnRules(newPlayGround, isSpawn);

        return resultPlayGround;
    }
}