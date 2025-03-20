using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CellularAutomata;

public sealed class SandRuleSetArray : IRuleSet
{
    private Vector dimension;
    private bool isInitialized = false;
    
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
        return this.ApplyRules(playGround, (position.X, position.Y));
    }
    
    public CellState ApplyRules(IPlayGround playGround, (int X, int Y) position)
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
        
        if (IsSolid(cellState))
        {
            //RuleCounter["Solid"]++;
            return CellState.Solid;
        }
        
        
        // We are sure. That cell is empty.
        
        // Prio 1: grain above me
        if (IsSand(cellNeighbors.Top))
        {
            //RuleCounter["Prio1"]++;
            return cellNeighbors.Top;
        }

        // Prio 2: grain to the top left, but only if its Prio 1 is blocked.
        if (IsSand(cellNeighbors.TopLeft) && (IsSand(cellNeighbors.Left) || cellNeighbors.Left == CellState.Solid) &&
            cellNeighbors.Top == CellState.Empty)
        {
            //RuleCounter["Prio2"]++;
            return cellNeighbors.TopLeft;
        }

        // Prio 3: grain to the top right, but only if its Prio 1 and Prio 2 is blocked.
        var cellNeighborsFromRight = GetNeighboursState(playGround, (position.X + 1, position.Y));
        if (IsSand(cellNeighbors.TopRight) &&
            cellNeighbors.Top == CellState.Empty &&
            (IsSand(cellNeighbors.Right) || cellNeighbors.Right == CellState.Solid) &&
            (IsSand(cellNeighborsFromRight.Right) || cellNeighborsFromRight.Right == CellState.Solid || 
             (cellNeighborsFromRight.Right == CellState.Empty && cellNeighborsFromRight.TopRight != CellState.Empty)))
        {
            //RuleCounter["Prio3"]++;
            return cellNeighbors.TopRight; 
        }

        //RuleCounter["Empty"]++;
        return CellState.Empty;
    }
    
    public IPlayGround ApplySpawnRules(IPlayGround playGround, bool isSpawn)
    {
        var localPlayGround = (PlayGroundArray)playGround;
        
        if (isSpawn)
        {
            var position = (localPlayGround.Dimension.X / 2, 0);
            var cellNeighbors = GetNeighboursState(localPlayGround, position);

            if (cellNeighbors.Bottom == CellState.Empty)
            {
                localPlayGround[position] = CellState.Sand;    
            }    
        }
        
        return localPlayGround;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CellNeighbors GetNeighboursState(IPlayGround playGround, (int X, int Y) position)
    {
        var posX = position.X;
        var posY = position.Y;
        (int X, int Y) topLeft = (posX - 1, posY - 1);
        (int X, int Y) top = (posX, posY - 1);
        (int X, int Y) topRight = (posX + 1, posY - 1);
        (int X, int Y) left = (posX - 1, posY);
        (int X, int Y) leftleft = (posX - 2, posY);
        (int X, int Y) right = (posX + 1, posY);
        (int X, int Y) bottomLeft = (posX - 1, posY + 1);
        (int X, int Y) bottom = (posX, posY + 1);
        (int X, int Y) bottomRight = (posX + 1, posY + 1);
        
        
        return new CellNeighbors(
            TopLeft: IsWithinBounds(topLeft.X, topLeft.Y) ? playGround[topLeft] : CellState.Empty,
            Top: IsWithinBounds(top.X, top.Y) ? playGround[top] : CellState.Empty,
            TopRight: IsWithinBounds(topRight.X, topRight.Y) ? playGround[topRight] : CellState.Empty,
            Left: IsWithinBounds(left.X, left.Y) ? playGround[left] : CellState.Empty,
            LeftLeft: IsWithinBounds(left.X - 1, left.Y) ? playGround[leftleft] : CellState.Empty,
            Right: IsWithinBounds(right.X, right.Y) ? playGround[right] : CellState.Empty,
            BottomLeft: IsWithinBounds(bottomLeft.X, bottomLeft.Y) ? playGround[bottomLeft] : CellState.Empty,
            Bottom: IsWithinBounds(bottom.X, bottom.Y) ? playGround[bottom] : CellState.Empty,
            BottomRight: IsWithinBounds(bottomRight.X, bottomRight.Y) ? playGround[bottomRight] : CellState.Empty
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(int x, int y )
    {
        var withinX = (uint)x < (uint)dimension.X; 
        var withinY = (uint)y < (uint)dimension.Y;
    
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