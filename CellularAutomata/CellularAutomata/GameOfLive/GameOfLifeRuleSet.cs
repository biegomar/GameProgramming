using System.Runtime.CompilerServices;
using CellularAutomata.Cells;
using CellularAutomata.Interfaces;
using CellularAutomata.MaterialFlow;

namespace CellularAutomata.GameOfLive;

public sealed class GameOfLifeRuleSet(Vector dimension) : IRuleSet
{
    private readonly Random random = new ();
    
    private static readonly (int DX, int DY)[] NeighborOffsets = 
    {
        (-1, -1), (-1, 0), (-1, 1),
        ( 0, -1),          ( 0, 1),
        ( 1, -1), ( 1, 0), ( 1, 1),
    };
    
    public CellBrightness ApplyRules(IPlayGround playGround, Vector position)
    {
        var cellState = playGround[position];
        
        var liveNeighbors = CountLivingNeighbors(playGround, position);
        
        return liveNeighbors == 3 || (cellState == CellBrightness.Solid && liveNeighbors == 2) ? CellBrightness.Solid : CellBrightness.Empty;
    }

    public MaterialMovement? ApplyMaterialRules(IPlayGround playGround, Vector position)
    {
        var liveNeighbors = CountLivingNeighbors(playGround, position);
        
        return liveNeighbors == 3 || (liveNeighbors == 2 && playGround.GetCell(position).Type == CellType.Solid) 
            ? new MaterialMovement(new Material(position, new Cell(CellType.Solid, CellBrightness.Solid)), null) 
            : new MaterialMovement(new Material(position, new Cell(CellType.Empty, CellBrightness.Empty)), null);
    }

    public IPlayGround ApplySpawnRules(IPlayGround playGround, Vector spawnPosition, Vector brushSize, double probability)
    {
        var startX = spawnPosition.X;
        var endX = spawnPosition.X + brushSize.X - 1;
        
        var startY = spawnPosition.Y;
        var endY = spawnPosition.Y + brushSize.Y - 1;

        for (var x = startX; x <= endX; x++)
        {
            for (var y = startY; y <= endY; y++)
            {
                var newPos = new Vector(x, y);
                if (IsWithinBounds(newPos) && playGround[newPos] == CellBrightness.Empty)
                {
                    if (random.NextDouble() < probability)
                    {
                        playGround[newPos] = CellBrightness.Solid;   
                    }
                }
            }    
        }
        
        return playGround;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int CountLivingNeighbors(IPlayGround playGround, Vector position)
    {
        int liveNeighbors = 0;
    
        foreach (var (dx, dy) in NeighborOffsets)
        {
            var neighbor = new Vector(position.X + dx, position.Y + dy);
    
            if (IsWithinBounds(neighbor) && playGround.GetCell(neighbor).Type == CellType.Solid)
            {
                liveNeighbors++;
                if (liveNeighbors == 4)
                    break;
            }
        }
    
        return liveNeighbors;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector neighbor)
    {
        return (uint)neighbor.X < (uint)dimension.X && (uint)neighbor.Y < (uint)dimension.Y;
    }
}