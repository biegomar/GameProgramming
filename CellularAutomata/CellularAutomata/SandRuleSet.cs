using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CellularAutomata;

public sealed class SandRuleSet : IRuleSet
{
    public IDictionary<string, uint> RuleCounter { get; init; } = new Dictionary<string, uint>();
    
    [StructLayout(LayoutKind.Sequential, Size = 9)]
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
    
    public CellState ApplyRules(IPlayGround playGround, Vector position)
    {
        var cellState = playGround[position];

        if (cellState == CellState.Solid)
        {
            return CellState.Solid;
        }

        var cellNeighbors = GetNeighboursState(playGround, position);
        
        if (IsSand(cellState))
        {
            if (
                (cellNeighbors.Bottom == CellState.Empty ||
                 cellNeighbors is { BottomRight: CellState.Empty, Right: CellState.Empty } 
                     or { BottomLeft: CellState.Empty, Left: CellState.Empty, LeftLeft: CellState.Empty })
                && position.Y < playGround.Dimension.Y - 1
               )
            {
                return CellState.Empty;
            }
            
            return cellState;
        }
           
        // Prio 1: grain above me
        if (IsSand(cellNeighbors.Top))
        {
            return cellNeighbors.Top;
        }

        // Prio 2: grain to the top left, but only if its Prio 1 is blocked.
        if (IsSand(cellNeighbors.TopLeft) && (IsSand(cellNeighbors.Left) || cellNeighbors.Left == CellState.Solid) &&
            cellNeighbors.Top == CellState.Empty)
        {
            return cellNeighbors.TopLeft;
        }

        // Prio 3: grain to the top right, but only if its Prio 1 and Prio 2 is blocked.
        var cellNeighborsFromRight = GetNeighboursState(playGround, new Vector(position.X + 1, position.Y));
        if (IsSand(cellNeighbors.TopRight) &&
            cellNeighbors.Top == CellState.Empty &&
            (IsSand(cellNeighbors.Right) || cellNeighbors.Right == CellState.Solid) &&
            (IsSand(cellNeighborsFromRight.Right) || cellNeighborsFromRight.Right == CellState.Solid || 
             (cellNeighborsFromRight.Right == CellState.Empty && cellNeighborsFromRight.TopRight != CellState.Empty)))
        {
            return cellNeighbors.TopRight; 
        }

        return CellState.Empty;
    }

    public CellState ApplyRules(IPlayGround playGround, (int X, int Y) position)
    {
        return ApplyRules(playGround, new Vector(position.X, position.Y));
    }

    public IPlayGround ApplySpawnRules(IPlayGround playGround, bool isSpawn)
    {
        var localPlayGround = (PlayGround)playGround;
        
        if (isSpawn)
        {
            
            var position = new Vector(localPlayGround.Dimension.X / 2, 0);
            var cellNeighbors = GetNeighboursState(localPlayGround, position);

            if (cellNeighbors.Bottom == CellState.Empty)
            {
                localPlayGround[position] = CellState.Sand;    
            }    
        }
        
        return localPlayGround;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CellNeighbors GetNeighboursState(IPlayGround playGround, Vector position)
    {
        var topLeft = new Vector(position.X - 1, position.Y - 1);
        var top = new Vector(position.X, position.Y - 1);
        var topRight = new Vector(position.X + 1, position.Y - 1);
        var left = new Vector(position.X - 1, position.Y);
        var leftleft = new Vector(position.X - 2, position.Y);
        var right = new Vector(position.X + 1, position.Y);
        var bottomLeft = new Vector(position.X - 1, position.Y + 1);
        var bottom = new Vector(position.X, position.Y + 1);
        var bottomRight = new Vector(position.X + 1, position.Y + 1);
        
        return new CellNeighbors(
            TopLeft: IsWithinBounds(playGround.Dimension, topLeft) ? playGround[topLeft] : CellState.Empty,
            Top: IsWithinBounds(playGround.Dimension, top) ? playGround[top] : CellState.Empty,
            TopRight: IsWithinBounds(playGround.Dimension, topRight) ? playGround[topRight] : CellState.Empty,
            Left: IsWithinBounds(playGround.Dimension, left) ? playGround[left] : CellState.Empty,
            LeftLeft: IsWithinBounds(playGround.Dimension, leftleft) ? playGround[leftleft] : CellState.Empty,
            Right: IsWithinBounds(playGround.Dimension, right) ? playGround[right] : CellState.Empty,
            BottomLeft: IsWithinBounds(playGround.Dimension, bottomLeft) ? playGround[bottomLeft] : CellState.Empty,
            Bottom: IsWithinBounds(playGround.Dimension, bottom) ? playGround[bottom] : CellState.Empty,
            BottomRight: IsWithinBounds(playGround.Dimension, bottomRight) ? playGround[bottomRight] : CellState.Empty
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector dimension, Vector position )
    {
        var withinX = (uint)position.X < (uint)dimension.X; 
        var withinY = (uint)position.Y < (uint)dimension.Y;
    
        return withinX && withinY;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsSand(CellState cellState)
    {
        return (byte)cellState > 1;
    }
}