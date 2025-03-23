using System.Numerics;
using System.Runtime.CompilerServices;

namespace CellularAutomata;

public sealed class GameOfLifeRuleSet(Vector dimension) : IRuleSet
{
    private const CellState Solid = CellState.Solid;
    private const CellState Empty = CellState.Empty;
    
    private static readonly (int DX, int DY)[] NeighborOffsets = 
    {
        (-1, -1), (-1, 0), (-1, 1),
        ( 0, -1),          ( 0, 1),
        ( 1, -1), ( 1, 0), ( 1, 1),
    };
    
    public IDictionary<string, uint> RuleCounter { get; init; } = new Dictionary<string, uint>
    {
        ["CellEmpty"] = 0,
        ["CellAlive"] = 0
    };

    
    public CellState ApplyRules(IPlayGround playGround, Vector position)
    {
        var cellState = playGround[position];

        var liveNeighbors = CountLivingNeighbors(playGround, position.X, position.Y);
        
        return liveNeighbors == 3 || (cellState == Solid && liveNeighbors == 2) ? Solid : Empty;
    }
    
    public IPlayGround ApplySpawnRules(IPlayGround playGround, Vector spawnPosition, Vector brushSize)
    {
        var startX = spawnPosition.X;
        var endX = spawnPosition.X + brushSize.X;
        
        var startY = spawnPosition.Y;
        var endY = spawnPosition.Y + brushSize.Y;

        for (var x = startX; x <= endX; x++)
        {
            for (var y = startY; y <= endY; y++)
            {
                var newPos = new Vector(x, y);
                if (IsWithinBounds(x, y) && playGround[newPos] == Empty)
                {
                    playGround[newPos] = Solid;
                }
            }    
        }
        
        return playGround;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int CountLivingNeighbors(IPlayGround playGround, int x, int y)
    {
        var liveNeighbors = 0;
    
        foreach (var (dx, dy) in NeighborOffsets)
        {
            var nx = x + dx;
            var ny = y + dy;
    
            if (IsWithinBounds(nx, ny) && playGround[(nx, ny)] == Solid)
            {
                liveNeighbors++;
                if (liveNeighbors == 4)
                    break;
            }
        }
    
        return liveNeighbors;
    }
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(int x, int y)
    {
        var withinX = (uint)x < (uint)dimension.X; 
        var withinY = (uint)y < (uint)dimension.Y;

        return withinX && withinY;
    }
}