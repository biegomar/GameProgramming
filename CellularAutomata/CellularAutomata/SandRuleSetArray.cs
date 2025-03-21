using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CellularAutomata;

public sealed class SandRuleSetArray : IRuleSet
{
    private Vector dimension;
    private bool isInitialized = false;
    private const CellState Solid = CellState.Solid;
    private const CellState Empty = CellState.Empty;
    
    public IDictionary<string, uint> RuleCounter { get; init; } = new Dictionary<string, uint>
    {
        // ["Solid"] = 0,
        // ["Prio0"] = 0,
        // ["Prio1"] = 0,
        // ["Prio2"] = 0,
        // ["Prio3"] = 0,
        // ["Empty"] = 0,
    };


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

    public CellState ApplyRules(IPlayGround playGround, Vector position)
    {
        var cellState = playGround[position];
        
        if (!isInitialized)
        {
            dimension = playGround.Dimension;
            isInitialized = true;
        }

        var cellNeighbors = GetNeighboursState(playGround, position);
        
        // First look at a cell with state

        if (IsSand(cellState))
        {
            //RuleCounter["Prio0"]++;
            if (
                (cellNeighbors.Bottom == Empty ||
                 cellNeighbors is { BottomRight: Empty, Right: Empty } 
                     or { BottomLeft: Empty, Left: Empty, LeftLeft: Empty })
                && position.Y < playGround.Dimension.Y - 1
               )
            {
                return Empty;
            }
            
            return cellState;
        }
        
        if (IsSolid(cellState))
        {
            //RuleCounter["Solid"]++;
            return Solid;
        }
        
        
        // We are sure. That cell is empty.
        
        // Prio 1: grain above me
        if (IsSand(cellNeighbors.Top))
        {
            //RuleCounter["Prio1"]++;
            return cellNeighbors.Top;
        }

        // Prio 2: grain to the top left, but only if its Prio 1 is blocked.
        if (IsSand(cellNeighbors.TopLeft) && (IsSand(cellNeighbors.Left) || cellNeighbors.Left == Solid) &&
            cellNeighbors.Top == Empty)
        {
            //RuleCounter["Prio2"]++;
            return cellNeighbors.TopLeft;
        }

        // Prio 3: grain to the top right, but only if its Prio 1 and Prio 2 is blocked.
        var cellNeighborsFromRight = GetNeighboursState(playGround, position);
        if (IsSand(cellNeighbors.TopRight) &&
            cellNeighbors.Top == Empty &&
            (IsSand(cellNeighbors.Right) || cellNeighbors.Right == Solid) &&
            (IsSand(cellNeighborsFromRight.Right) || cellNeighborsFromRight.Right == Solid || 
             (cellNeighborsFromRight.Right == Empty && cellNeighborsFromRight.TopRight != Empty)))
        {
            //RuleCounter["Prio3"]++;
            return cellNeighbors.TopRight; 
        }

        //RuleCounter["Empty"]++;
        return Empty;
    }
    
    public IPlayGround ApplySpawnRules(IPlayGround playGround, bool isSpawn, Vector spawnPosition)
    {
        return playGround;
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
}