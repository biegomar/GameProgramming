using System.Linq;

namespace CellularAutomata;

public class GameOfLifeRuleSet : IRuleSet<bool>
{
    public bool ApplyRules(PlayGround<bool> playGround, Vector position)
    {
        var isAlive = playGround[position];
        
        var liveNeighbors = CountLiveNeighbors(playGround, position);
        
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

    public bool ApplyRules(PlayGroundArray<bool> playGround, (int X, int Y, int Z) position)
    {
        return this.ApplyRules(playGround, new Vector(position.X, position.Y, position.Z));
    }

    public PlayGround<bool> ApplySpawnRules(PlayGround<bool> playGround, bool isSpawn)
    {
        return playGround;
    }

    public PlayGroundArray<bool> ApplySpawnRules(PlayGroundArray<bool> playGround, bool isSpawn)
    {
        throw new NotImplementedException();
    }

    private int CountLiveNeighbors(PlayGround<bool> playGround, Vector position)
    {
        var neighbors = new List<Vector>
        {
            new Vector(position.X - 1, position.Y - 1, 0), 
            new Vector(position.X,     position.Y - 1, 0), 
            new Vector(position.X + 1, position.Y - 1, 0), 
            new Vector(position.X - 1, position.Y,     0), 
            new Vector(position.X + 1, position.Y,     0), 
            new Vector(position.X - 1, position.Y + 1, 0), 
            new Vector(position.X,     position.Y + 1, 0), 
            new Vector(position.X + 1, position.Y + 1, 0)  
        };
        
        return neighbors.Count(vec =>
            IsWithinBounds(playGround.Dimension,vec) &&
            playGround[vec] 
        );
    }
    
    private bool IsWithinBounds(Vector dimension, Vector position)
    {
        return position.X >= 0 && position.Y >= 0 &&
               position.X < dimension.X &&
               position.Y < dimension.Y;
    }
}