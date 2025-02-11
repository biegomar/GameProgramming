namespace CellularAutomata;

public static class AutomataArray<T>
{
    public static PlayGroundArray<T> NextGeneration(PlayGroundArray<T> initialPlayGround, IRuleSet<T> ruleSet, bool isSpawn)
    {
        var newPlayGround = new PlayGroundArray<T>(initialPlayGround.Dimension);

        foreach (var cell in initialPlayGround.Cells)
        {
            newPlayGround[cell.Position] = ruleSet.ApplyRules(initialPlayGround, cell.Position); 
        }
        
        // for (var x = 0; x < initialPlayGround.Dimension.X; x++)
        // {
        //     for (var y = 0; y < initialPlayGround.Dimension.Y; y++)
        //     {
        //         for (var z = 0; z <= initialPlayGround.Dimension.Z; z++)
        //         {
        //             newPlayGround[(x, y, z)] = ruleSet.ApplyRules(initialPlayGround, (x, y, z));
        //             
        //             if (initialPlayGround.Dimension.Z == 0) break;
        //         }
        //     }
        // }

        
        var resultPlayGround = (PlayGroundArray<T>)ruleSet.ApplySpawnRules(newPlayGround, isSpawn);
        
        return resultPlayGround;
    }
    
    public static PlayGroundArray<T> NextGenerationParallel(PlayGroundArray<T> initialPlayGround, IRuleSet<T> ruleSet, bool isSpawn)
    {
        var newPlayGround = new PlayGroundArray<T>(initialPlayGround.Dimension);

        Parallel.ForEach(initialPlayGround.Cells.Cast<Cell<T>>(), cell =>
        {
            newPlayGround[cell.Position] = ruleSet.ApplyRules(initialPlayGround, cell.Position);
        });

        
        // Parallel.For(0, (int)initialPlayGround.Dimension.X , x =>
        // {
        //     for (var y = 0; y < initialPlayGround.Dimension.Y; y++)
        //     {
        //         for (var z = 0; z <= initialPlayGround.Dimension.Z; z++)
        //         {
        //             newPlayGround[(x, y, z)] = ruleSet.ApplyRules(initialPlayGround, (x, y, z));
        //             
        //             if (initialPlayGround.Dimension.Z == 0) break;
        //         }
        //     }
        // });

        var resultPlayGround = (PlayGroundArray<T>)ruleSet.ApplySpawnRules(newPlayGround, isSpawn);

        return resultPlayGround;
    }
}