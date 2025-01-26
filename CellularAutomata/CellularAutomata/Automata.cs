namespace CellularAutomata;

public static class Automata<T>
{
    public static PlayGround<T> NextGeneration(PlayGround<T> initialPlayGround, IRuleSet<T> ruleSet)
    {
        var newPlayGround = new PlayGround<T>(initialPlayGround.Dimension);
        var position = Vector.Zero;
        
        for (var x = 0; x < initialPlayGround.Dimension.X; x++)
        {
            for (var y = 0; y < initialPlayGround.Dimension.Y; y++)
            {
                position.X = x;
                position.Y = y;
                newPlayGround[position] = ruleSet.ApplyRules(initialPlayGround, position);
            }
        }
        
        return newPlayGround;
    }
}