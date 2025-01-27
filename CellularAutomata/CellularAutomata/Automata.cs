namespace CellularAutomata;

public static class Automata<T>
{
    public static PlayGround<T> NextGeneration(PlayGround<T> initialPlayGround, IRuleSet<T> ruleSet)
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
        
        return newPlayGround;
    }
}