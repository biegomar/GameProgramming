using System.Linq;

namespace CellularAutomata;

public class GameOfLifeRuleSetArray : IRuleSet<bool>
{
    public IDictionary<string, uint> RuleCounter { get; init; } = new Dictionary<string, uint>
    {
        ["CellEmpty"] = 0,
        ["CellAlive"] = 0
    };
    
    public bool ApplyRules(IPlayGround<bool> playGround, Vector position)
    {
        return this.ApplyRules(playGround, ((int)position.X, (int)position.Y, (int)position.Z));
    }

    public bool ApplyRules(IPlayGround<bool> playGround, (int X, int Y, int Z) position)
    {
        var cellState = playGround[position];
        
        var liveNeighbors = CountLivingNeighbors(playGround, position);
        
        return liveNeighbors == 3 || (cellState && liveNeighbors == 2);
    }
    
    private int CountLivingNeighbors(IPlayGround<bool> playGround, (int X, int Y, int Z) position)
    {
        int liveNeighbors = 0;
        
        bool shouldBreak = false;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;
                
                var neighbor = (position.X + dx, position.Y + dy, 0);
                
                if (IsWithinBounds(playGround.Dimension, neighbor) && playGround[neighbor])
                {
                    liveNeighbors++;
                    if (liveNeighbors == 4)
                    {
                        shouldBreak = true;
                        break;

                    }
                }
            }
            
            if (shouldBreak) break;

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