namespace CellularAutomata;

public class SandRuleSet : IRuleSet<bool>
{
    public bool ApplyRules(PlayGround<bool> playGround, Vector position)
    {
        var isSand = playGround[position];

        if (isSand)
        {
            var (left, middle, right) = GetEmptyNeighboursBelow(playGround, position);
        
            return !(left != null || right != null || middle != null);
        }
        else
        {
           var (topLeft, left, topMiddle, right, topRight, topRightRight, rightRight) = GetNeighboursState(playGround, position);
           
           // Prio 1: grain above me
           if (topMiddle)
           {
               return true;
           }

           // Prio 2: grain to the top left, but only if its Prio 1 is blocked.
           if (left && topLeft)
           {
               return true;
           }

           // Prio 3: grain to the top right, but only if its Prio 1 and Prio 2 is blocked.
           if (right && topRight && (topRightRight || rightRight))
           {
               return true;
           }

           return false;
        }
    }

    public PlayGround<bool> ApplySpawnRules(PlayGround<bool> playGround)
    {
        var position = new Vector(playGround.Dimension.X / 2, 0, 0);
        var (_, middle,_) = GetEmptyNeighboursBelow(playGround, position);

        playGround[position] = middle != null;
        
        return playGround;
    }

    private (Vector? left, Vector? middle, Vector? right) GetEmptyNeighboursBelow(PlayGround<bool> playGround, Vector position)
    {
        var left = new Vector(position.X - 1, position.Y + 1, 0);
        var middle = new Vector(position.X, position.Y + 1, 0);
        var right = new Vector(position.X + 1, position.Y + 1, 0);

        return (IsWithinBounds(playGround.Dimension, left) && !playGround[left] ? left : null, 
            IsWithinBounds(playGround.Dimension, middle) && !playGround[middle] ? middle : null,
            IsWithinBounds(playGround.Dimension, right) && !playGround[right] ? right : null);
    }
    
    private (bool topLeft, bool left, bool topMiddle, bool right, bool topRight, bool topRightRight, bool rightRight) GetNeighboursState(PlayGround<bool> playGround, Vector position)
    {
        var left = new Vector(position.X - 1, position.Y - 1, 0);
        var leftMiddle = new Vector(position.X - 1, position.Y, 0);
        var middle = new Vector(position.X, position.Y - 1, 0);
        var rightMiddle = new Vector(position.X + 1, position.Y, 0);
        var right = new Vector(position.X + 1, position.Y - 1, 0);
        var rightRightMiddle = new Vector(position.X + 2, position.Y, 0);
        var rightRight = new Vector(position.X + 2, position.Y - 1, 0);
        
        return (IsWithinBounds(playGround.Dimension, left) && playGround[left],
            IsWithinBounds(playGround.Dimension, leftMiddle) && playGround[leftMiddle],
            IsWithinBounds(playGround.Dimension, middle) && playGround[middle],
            IsWithinBounds(playGround.Dimension, rightMiddle) && playGround[rightMiddle],
            IsWithinBounds(playGround.Dimension, right) && playGround[right],
            IsWithinBounds(playGround.Dimension, rightRightMiddle) && playGround[rightRightMiddle],
            IsWithinBounds(playGround.Dimension, rightRight) && playGround[rightRight]
            );
    }
    
    private bool IsWithinBounds(Vector dimension, Vector position)
    {
        return position.X >= 0 && position.Y >= 0 &&
               position.X < dimension.X &&
               position.Y < dimension.Y;
    }
}