namespace CellularAutomata;

public static class Automata<T>
{
    public static PlayGround<T> NextGeneration(PlayGround<T> initialPlayGround, IRuleSet<T> ruleSet)
    {
        var newPlayGround = new PlayGround<T>(initialPlayGround.Dimension);
        
        for (var x = 0; x < initialPlayGround.Dimension.X; x++)
        {
            for (var y = 0; y < initialPlayGround.Dimension.Y; y++)
            {
                var position = new Vector(x,y,0);
                newPlayGround[position] = ruleSet.ApplyRules(initialPlayGround, position);
            }
        }
        
        return newPlayGround;
    }
}