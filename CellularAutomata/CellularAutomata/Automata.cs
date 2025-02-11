namespace CellularAutomata;

public static class Automata<T>
{
    public static PlayGround<T> NextGeneration(PlayGround<T> initialPlayGround, IRuleSet<T> ruleSet, bool isSpawn)
    {
        var newPlayGround = new PlayGround<T>(initialPlayGround.Dimension);

        foreach (var cell in initialPlayGround.Cells)
        {
            newPlayGround[cell.Key] = ruleSet.ApplyRules(initialPlayGround, cell.Key); 
        }
        
        var resultPlayGround = (PlayGround<T>)ruleSet.ApplySpawnRules(newPlayGround, isSpawn);
        
        return resultPlayGround;
    }

    public static PlayGround<T> NextGenerationParallel(PlayGround<T> initialPlayGround, IRuleSet<T> ruleSet, bool isSpawn)
    {
        var newPlayGround = new PlayGround<T>(initialPlayGround.Dimension);
        
        Parallel.ForEach(initialPlayGround.Cells, cell =>
        {
            newPlayGround[cell.Key] = ruleSet.ApplyRules(initialPlayGround, cell.Key); 
        });

        var resultPlayGround = (PlayGround<T>)ruleSet.ApplySpawnRules(newPlayGround, isSpawn);
        
        return resultPlayGround;
    }
    
    public static PlayGround<T> NextGenerationForLoop(PlayGround<T> initialPlayGround, IRuleSet<T> ruleSet, bool isSpawn)
    {
        var newPlayGround = new PlayGround<T>(initialPlayGround.Dimension);
        var dimensionX = (int)initialPlayGround.Dimension.X;
        var dimensionY = (int)initialPlayGround.Dimension.Y;
        var position = Vector.Zero;
        
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
}