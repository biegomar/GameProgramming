using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CellularAutomata;

public sealed class SandRuleSet(Vector dimension) : IRuleSet
{
    private const CellState Solid = CellState.Solid;
    private const CellState Empty = CellState.Empty;
    private readonly Random random = new ();
    
    [StructLayout(LayoutKind.Sequential, Size = 9, Pack = 1)]
    private record struct CellNeighbors(
        CellState TopLeft,
        CellState Top,
        CellState TopRight,
        CellState Left,
        CellState LeftLeft,
        CellState Right,
        CellState BottomLeft,
        CellState Bottom,
        CellState BottomRight
    );
    
    [StructLayout(LayoutKind.Sequential, Size = 4, Pack = 1)]
    private record struct TopRowNeighbors(
        CellState TopLeft,
        CellState Top,
        CellState TopRight
    );
    
    [StructLayout(LayoutKind.Sequential, Size = 6, Pack = 1)]
    private record struct PushCellNeighbors(
        CellState Left,
        CellState LeftOpponent,
        CellState Right,
        CellState BottomLeft,
        CellState Bottom,
        CellState BottomRight
    );
    
    [StructLayout(LayoutKind.Sequential, Size = 6, Pack = 1)]
    private record struct RightOpponentCellNeighbors(
        CellState Top,
        CellState Opponent
    );

    public CellState ApplyRules(IPlayGround playGround, Vector position)
    {
        var cellState = playGround[position];

        var pushCellNeighbors = GetPushCellNeighboursState(playGround, position);
        
        // First look at a cell with state - so we push the grain.

        if (IsSand(cellState))
        {
            if (pushCellNeighbors.Bottom == Empty)
            {
                return Empty;
            }

            if (pushCellNeighbors is { BottomRight: Empty, Right: Empty } && position.Y < playGround.Dimension.Y - 1)
            {
                if (pushCellNeighbors is { BottomLeft: Empty, Left: Empty } && !playGround.IsProcessedRight(new Vector(position.X - 2, position.Y)) && position.Y < playGround.Dimension.Y - 1)
                {
                    if (WillMoveRight())
                    {
                        playGround.MarkAsProcessedRight(position); 
                    }
                    else
                    {
                        playGround.MarkAsProcessedLeft(position);
                    }
                    return Empty;
                }
                
                playGround.MarkAsProcessedRight(position);
                return Empty;
            }
            
            if (pushCellNeighbors is { BottomLeft: Empty, Left: Empty } && !playGround.IsProcessedRight(new Vector(position.X - 2, position.Y)) && position.Y < playGround.Dimension.Y - 1)
            {
                playGround.MarkAsProcessedLeft(position);
                return Empty;
            }
            
            return cellState;
        }
        
        if (IsSolid(cellState))
        {
            return Solid;
        }
        
        // We are sure. That cell is empty. Now we pull the grain.
        
        var topRowNeighbors = GetTopRowNeighborsState(playGround, position);
        
        // Prio 1: grain above me
        if (IsSand(topRowNeighbors.Top))
        {
            return topRowNeighbors.Top;
        }
        
        if (playGround.IsProcessedRight(new Vector(position.X - 1, position.Y - 1)))
        {
            return topRowNeighbors.TopLeft;
        }
        
        if (playGround.IsProcessedLeft(new Vector(position.X + 1, position.Y - 1)))
        {
            return topRowNeighbors.TopRight;
        }
        
        return Empty;
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
                if (IsWithinBounds(new Vector(x, y)) && playGround[new Vector(x, y)] == Empty)
                {
                    if (random.NextDouble() < probability)
                    {
                        playGround[new Vector(x, y)] = GetRandomSandCellState();   
                    }
                }
            }    
        }
        
        return playGround;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CellNeighbors GetNeighboursState(IPlayGround playGround, Vector position)
    {
        return new (
            TopLeft: IsWithinBounds(new Vector(position.X - 1, position.Y - 1)) ? playGround[new Vector(position.X - 1, position.Y - 1)] : Solid,
            Top: IsWithinBounds(new Vector(position.X, position.Y - 1)) ? playGround[new Vector(position.X, position.Y - 1)] : Solid,
            TopRight: IsWithinBounds(new Vector(position.X + 1, position.Y - 1)) ? playGround[new Vector(position.X + 1, position.Y - 1)] : Solid,
            Left: IsWithinBounds(new Vector(position.X - 1, position.Y)) ? playGround[new Vector(position.X - 1, position.Y)] : Solid,
            LeftLeft: IsWithinBounds(new Vector(position.X - 2, position.Y)) ? playGround[new Vector(position.X - 2, position.Y)] : Solid,
            Right: IsWithinBounds(new Vector(position.X + 1, position.Y)) ? playGround[new Vector(position.X + 1, position.Y)] : Solid,
            BottomLeft: IsWithinBounds(new Vector(position.X - 1, position.Y + 1)) ? playGround[new Vector(position.X - 1, position.Y + 1)] : Solid,
            Bottom: IsWithinBounds(new Vector(position.X, position.Y + 1)) ? playGround[new Vector(position.X, position.Y + 1)] : Solid,
            BottomRight: IsWithinBounds(new Vector(position.X + 1, position.Y + 1)) ? playGround[new Vector(position.X + 1, position.Y + 1)] : Solid
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private RightOpponentCellNeighbors GetRightOpponentCellNeighborsState(IPlayGround playGround, Vector position)
    {
        return new RightOpponentCellNeighbors(
            Opponent: IsWithinBounds(new Vector(position.X + 2, position.Y)) ? playGround[new Vector(position.X + 2, position.Y)] : Solid,
            Top: IsWithinBounds(new Vector(position.X + 2, position.Y - 1)) ? playGround[new Vector(position.X + 2, position.Y - 1)] : Solid
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private PushCellNeighbors GetPushCellNeighboursState(IPlayGround playGround, Vector position)
    {
        return new PushCellNeighbors(
            Left: IsWithinBounds(new Vector(position.X - 1, position.Y)) ? playGround[new Vector(position.X - 1, position.Y)] : Solid,
            LeftOpponent: IsWithinBounds(new Vector(position.X - 2, position.Y)) ? playGround[new Vector(position.X - 2, position.Y)] : Solid,
            Right: IsWithinBounds(new Vector(position.X + 1, position.Y)) ? playGround[new Vector(position.X + 1, position.Y)] : Solid,
            BottomLeft: IsWithinBounds(new Vector(position.X - 1, position.Y + 1)) ? playGround[new Vector(position.X - 1, position.Y + 1)] : Solid,
            Bottom: IsWithinBounds(new Vector(position.X, position.Y + 1)) ? playGround[new Vector(position.X, position.Y + 1)] : Solid,
            BottomRight: IsWithinBounds(new Vector(position.X + 1, position.Y + 1)) ? playGround[new Vector(position.X + 1, position.Y + 1)] : Solid
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TopRowNeighbors GetTopRowNeighborsState(IPlayGround playGround, Vector position)
    {
        return new TopRowNeighbors(
            TopLeft: IsWithinBounds(new Vector(position.X - 1, position.Y - 1)) ? playGround[new Vector(position.X - 1, position.Y - 1)] : Solid,
            Top: IsWithinBounds(new Vector(position.X, position.Y - 1)) ? playGround[new Vector(position.X, position.Y - 1)] : Solid,
            TopRight: IsWithinBounds(new Vector(position.X + 1, position.Y - 1)) ? playGround[new Vector(position.X + 1, position.Y - 1)] : Solid
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector position )
    {
        return (uint)position.X < (uint)dimension.X && (uint)position.Y < (uint)dimension.Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsSand(CellState cellState)
    {
        return (byte)cellState > 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsSolid(CellState cellState)
    {
        return (byte)cellState == 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsSandOrSolid(CellState cellState)
    {
        return (byte)cellState > 0;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CellState GetRandomSandCellState()
    {
        var randomValue = random.Next(2, 6);

        return (CellState)randomValue;

    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool WillMoveRight()
    {
        return (Environment.TickCount & 1) == 0;
    }
}