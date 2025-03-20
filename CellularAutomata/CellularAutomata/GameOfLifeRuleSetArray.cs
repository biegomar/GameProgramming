using System.Runtime.CompilerServices;

namespace CellularAutomata;

public sealed class GameOfLifeRuleSetArray : IRuleSet
{
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
        return this.ApplyRules(playGround, (position.X, position.Y));
    }

    public CellState ApplyRules(IPlayGround playGround, (int X, int Y) position)
    {
        var cellState = playGround[position];
        
        var liveNeighbors = CountLivingNeighbors(playGround, position.X, position.Y);
        
        return liveNeighbors == 3 || (cellState == CellState.Solid && liveNeighbors == 2) ? CellState.Solid : CellState.Empty;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int CountLivingNeighbors(IPlayGround playGround, int X, int Y)
    {
        int liveNeighbors = 0;

        foreach (var (dx, dy) in NeighborOffsets)
        {
            var nx = X + dx;
            var ny = Y + dy;

            if (IsWithinBounds(playGround.Dimension, nx, ny) && playGround[(nx, ny)] == CellState.Solid)
            {
                liveNeighbors++;
                if (liveNeighbors == 4)
                    break;
            }
        }

        return liveNeighbors;
    }

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector dimension, int x, int y )
    {
        var withinX = (uint)x < (uint)dimension.X; 
        var withinY = (uint)y < (uint)dimension.Y;

        return withinX && withinY;
    }
    
    public IPlayGround ApplySpawnRules(IPlayGround playGround, bool isSpawn)
    {
        return playGround;
    }
}