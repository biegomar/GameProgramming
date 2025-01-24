using System.Linq;

namespace CellularAutomata;

public class GameOfLifeRuleSet : IRuleSet<bool>
{
    public PlayGround<bool> PlayGround { get; init; }

    public GameOfLifeRuleSet(PlayGround<bool> playGround)
    {
        this.PlayGround = playGround;
    }
    
    public bool ApplyRules(Vector position)
    {
        var isAlive = this.PlayGround[position];
        
        var liveNeighbors = CountLiveNeighbors(position);
        
        // Game of Life Rules
        if (isAlive)
        {
            // Dies due to underpopulation (<2) or overpopulation (>3)
            if (liveNeighbors < 2 || liveNeighbors > 3)
                return false; // Cell dies
            return true; // Cell stays alive
        }

        // Rebirth with exactly 3 neighbors alive
        if (liveNeighbors == 3)
            return true; // Cell comes to life
        return false; // Cell remains dead
    }
    
    private int CountLiveNeighbors(Vector position)
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
            IsWithinBounds(vec) &&
            this.PlayGround[vec] 
        );
    }
    
    private bool IsWithinBounds(Vector position)
    {
        return position.X >= 0 && position.Y >= 0 &&
               position.X < this.PlayGround.Dimension.X &&
               position.Y < this.PlayGround.Dimension.Y;
    }
}