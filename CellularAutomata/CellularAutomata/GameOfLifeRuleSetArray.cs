using System.Linq;

namespace CellularAutomata;

public class GameOfLifeRuleSetArray : IRuleSet<bool>
{
    public bool ApplyRules(IPlayGround<bool> playGround, Vector position)
    {
        return this.ApplyRules(playGround, ((int)position.X, (int)position.Y, (int)position.Z));
    }

    public bool ApplyRules(IPlayGround<bool> playGround, (int X, int Y, int Z) position)
    {
        var localPlayGround = (PlayGroundArray<bool>)playGround;
        var cellState = localPlayGround[position];
        
        var liveNeighbors = CountLivingNeighbors(localPlayGround, position);
        
        if (cellState && liveNeighbors is 2 or 3)
        {
            return true; 
        }

        if (!cellState && liveNeighbors == 3)
        {
            return true; 
        }
        
        return false;
    }

    private int CountLivingNeighbors(PlayGroundArray<bool> playGround, (int X, int Y, int Z) position)
    {
        var neighbors = new List<(int X, int Y, int Z)>
        {
            (position.X - 1, position.Y - 1, 0), 
            (position.X,     position.Y - 1, 0), 
            (position.X + 1, position.Y - 1, 0), 
            (position.X - 1, position.Y,     0), 
            (position.X + 1, position.Y,     0), 
            (position.X - 1, position.Y + 1, 0), 
            (position.X,     position.Y + 1, 0), 
            (position.X + 1, position.Y + 1, 0)  
        };
        
        return neighbors.Count(vec =>
            IsWithinBounds(playGround.Dimension,vec) &&
            playGround[vec] 
        );
    }
    
    private bool IsWithinBounds(Vector dimension, (int X, int Y, int Z) position)
    {
        return position.X >= 0 && position.Y >= 0 &&
               position.X < dimension.X &&
               position.Y < dimension.Y;
    }

    public IPlayGround<bool> ApplySpawnRules(IPlayGround<bool> playGround, bool isSpawn)
    {
        return playGround;
    }
}