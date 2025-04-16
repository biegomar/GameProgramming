using CellularAutomata.Cells;
using CellularAutomata.MaterialFlow;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.NoiseGrid;

public class NoiseGridRuleSet(Vector dimension) 
{
    private const CellBrightness Solid = CellBrightness.Solid;
    private const CellBrightness Empty = CellBrightness.Empty;
    
    private static readonly (int DX, int DY)[] NeighborOffsets = 
    {
        (-1, -1), (-1, 0), (-1, 1),
        ( 0, -1),          ( 0, 1),
        ( 1, -1), ( 1, 0), ( 1, 1),
    };
    
    public bool ApplyRules(SimplePlayGround playGround, Vector position)
    {
        var neighborWallCount = CountNeighborWalls(playGround, position);
        
        return neighborWallCount > 4;
    }
    
    private int CountNeighborWalls(SimplePlayGround playGround, Vector position)
    {
        var neighborWallCount = 0;
    
        foreach (var (dx, dy) in NeighborOffsets)
        {
            var neighbor = new Vector(position.X + dx, position.Y + dy);

            if (IsWithinBounds(neighbor))
            {
                if (playGround.GetState(neighbor))
                {
                    neighborWallCount++;
                }
            }
            else
            {
                neighborWallCount++;
            }
                
            if (neighborWallCount == 5)
                break;
        }
    
        return neighborWallCount;
    }
    
    private bool IsWithinBounds(Vector neighbor)
    {
        var withinX = (uint)neighbor.X < (uint)dimension.X; 
        var withinY = (uint)neighbor.Y < (uint)dimension.Y;

        return withinX && withinY;
    }
}