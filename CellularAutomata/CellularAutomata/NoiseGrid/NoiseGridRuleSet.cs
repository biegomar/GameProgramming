using CellularAutomata.Cells;
using CellularAutomata.Interfaces;
using CellularAutomata.MaterialFlow;

namespace CellularAutomata.NoiseGrid;

public class NoiseGridRuleSet(Vector dimension) : IRuleSet
{
    private const CellBrightness Solid = CellBrightness.Solid;
    private const CellBrightness Empty = CellBrightness.Empty;
    
    private static readonly (int DX, int DY)[] NeighborOffsets = 
    {
        (-1, -1), (-1, 0), (-1, 1),
        ( 0, -1),          ( 0, 1),
        ( 1, -1), ( 1, 0), ( 1, 1),
    };
    
    public CellBrightness ApplyRules(IPlayGround playGround, Vector position)
    {
        var neighborWallCount = CountNeighborWalls(playGround, position);
        
        return neighborWallCount > 4 ? Solid : Empty;
    }

    public MaterialMovement? ApplyMaterialRules(IPlayGround playGround, Vector position)
    {
        var neighborWallCount = CountNeighborWalls(playGround, position);
        
        return neighborWallCount > 4 
            ? new MaterialMovement(new Material(position, new Cell(CellType.Solid, CellBrightness.Solid)), null) 
            : new MaterialMovement(new Material(position, new Cell(CellType.Empty, CellBrightness.Empty)), null);
    }

    public IPlayGround ApplySpawnRules(IPlayGround playGround, Vector spawnPosition, Vector brushSize, double probability)
    {
        return playGround;
    }
    
    private int CountNeighborWalls(IPlayGround playGround, Vector position)
    {
        var neighborWallCount = 0;
    
        foreach (var (dx, dy) in NeighborOffsets)
        {
            var neighbor = new Vector(position.X + dx, position.Y + dy);

            if (IsWithinBounds(neighbor))
            {
                if (playGround.GetCell(neighbor).Type == CellType.Solid)
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