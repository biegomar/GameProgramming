using System.Linq;

namespace CellularAutomata;

public class GameOfLifeRuleSet : IRuleSet<bool>
{
    public IDictionary<string, uint> RuleCounter { get; init; } = new Dictionary<string, uint>
    {
        ["CellEmpty"] = 0,
        ["CellAlive"] = 0
    };

    
    public bool ApplyRules(IPlayGround<bool> playGround, (int X, int Y, int Z) position)
    {
        return this.ApplyRules(playGround, new Vector(position.X, position.Y, position.Z));
    }

    public bool ApplyRules(IPlayGround<bool> playGround, Vector position)
    {
        var localPlayGround = (PlayGround<bool>)playGround;
        var cellState = localPlayGround[position];
        
        var liveNeighbors = CountLivingNeighbors(localPlayGround, position);
        
        return liveNeighbors == 3 || (cellState && liveNeighbors == 2);
    }
    
    private int CountLivingNeighbors(PlayGround<bool> playGround, Vector position)
    {
        var liveNeighbors = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                var neighbor = ((int)position.X + dx, (int)position.Y + dy, 0);

                if (IsWithinBounds(playGround.Dimension, neighbor) && playGround[neighbor])
                {
                    liveNeighbors++;
                }
            }
        }

        return liveNeighbors;
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