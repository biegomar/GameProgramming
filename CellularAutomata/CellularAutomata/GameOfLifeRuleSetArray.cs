using System.Linq;

namespace CellularAutomata;

public class GameOfLifeRuleSetArray : IRuleSet<bool>
{
    public bool ApplyRules(PlayGround<bool> playGround, Vector position)
    {
        return this.ApplyRules((PlayGroundArray<bool>)playGround, ((int)position.X, (int)position.Y, (int)position.Z));
    }

    public bool ApplyRules(PlayGroundArray<bool> playGround, (int X, int Y, int Z) position)
    {
        var isAlive = playGround[position];
        
        var liveNeighbors = CountLivingNeighbors(playGround, position);
        
        if (isAlive && liveNeighbors is 2 or 3)
        {
            return true; 
        }

        if (!isAlive && liveNeighbors == 3)
        {
            return true; 
        }
        
        return false;
    }

    public PlayGround<bool> ApplySpawnRules(PlayGround<bool> playGround, bool isSpawn)
    {
        return playGround;
    }

    public PlayGroundArray<bool> ApplySpawnRules(PlayGroundArray<bool> playGround, bool isSpawn)
    {
        return playGround;
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
}