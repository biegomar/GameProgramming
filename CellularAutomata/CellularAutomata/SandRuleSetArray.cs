using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CellularAutomata;

public sealed class SandRuleSetArray : IRuleSet<SandCellState>
{
    public IDictionary<string, uint> RuleCounter { get; init; } = new Dictionary<string, uint>
    {
        ["Solid"] = 0,
        ["Prio0"] = 0,
        ["Prio1"] = 0,
        ["Prio2"] = 0,
        ["Prio3"] = 0,
        ["Empty"] = 0,
    };
    
    
    [StructLayout(LayoutKind.Sequential, Size = 9)]
    private record struct CellNeighbors(
        SandCellState TopLeft,
        SandCellState Top,
        SandCellState TopRight,
        SandCellState Left,
        SandCellState LeftLeft,
        SandCellState Right,
        SandCellState BottomLeft,
        SandCellState Bottom,
        SandCellState BottomRight
    );

    public SandCellState ApplyRules(IPlayGround<SandCellState> playGround, Vector position)
    {
        return this.ApplyRules(playGround, (position.X, position.Y));
    }
    
    public SandCellState ApplyRules(IPlayGround<SandCellState> playGround, (int X, int Y) position)
    {
        var cellState = playGround[position];

        var cellNeighbors = GetNeighboursState(playGround, position);
        
        if (IsSand(cellState))
        {
            //RuleCounter["Prio0"]++;
            if (
                (cellNeighbors.Bottom == SandCellState.Empty ||
                 cellNeighbors is { BottomRight: SandCellState.Empty, Right: SandCellState.Empty } 
                     or { BottomLeft: SandCellState.Empty, Left: SandCellState.Empty, LeftLeft: SandCellState.Empty })
                && position.Y < playGround.Dimension.Y - 1
               )
            {
                return SandCellState.Empty;
            }
            
            return cellState;
        }
        
        // Prio 1: grain above me
        if (IsSand(cellNeighbors.Top))
        {
            //RuleCounter["Prio1"]++;
            return cellNeighbors.Top;
        }

        // Prio 2: grain to the top left, but only if its Prio 1 is blocked.
        if (IsSand(cellNeighbors.TopLeft) && (IsSand(cellNeighbors.Left) || cellNeighbors.Left == SandCellState.Solid) &&
            cellNeighbors.Top == SandCellState.Empty)
        {
            //RuleCounter["Prio2"]++;
            return cellNeighbors.TopLeft;
        }

        // Prio 3: grain to the top right, but only if its Prio 1 and Prio 2 is blocked.
        var cellNeighborsFromRight = GetNeighboursState(playGround, (position.X + 1, position.Y));
        if (IsSand(cellNeighbors.TopRight) &&
            cellNeighbors.Top == SandCellState.Empty &&
            (IsSand(cellNeighbors.Right) || cellNeighbors.Right == SandCellState.Solid) &&
            (IsSand(cellNeighborsFromRight.Right) || cellNeighborsFromRight.Right == SandCellState.Solid || 
             (cellNeighborsFromRight.Right == SandCellState.Empty && cellNeighborsFromRight.TopRight != SandCellState.Empty)))
        {
            //RuleCounter["Prio3"]++;
            return cellNeighbors.TopRight; 
        }
        
        if (cellState == SandCellState.Solid)
        {
            //RuleCounter["Solid"]++;
            return SandCellState.Solid;
        }

        //RuleCounter["Empty"]++;
        return SandCellState.Empty;
    }
    
    public IPlayGround<SandCellState> ApplySpawnRules(IPlayGround<SandCellState> playGround, bool isSpawn)
    {
        var localPlayGround = (PlayGroundArray<SandCellState>)playGround;
        
        if (isSpawn)
        {
            
            var position = (localPlayGround.Dimension.X / 2, 0);
            var cellNeighbors = GetNeighboursState(localPlayGround, position);

            if (cellNeighbors.Bottom == SandCellState.Empty)
            {
                localPlayGround[position] = SandCellState.Sand;    
            }    
        }
        
        return localPlayGround;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CellNeighbors GetNeighboursState(IPlayGround<SandCellState> playGround, (int X, int Y) position)
    {
        (int X, int Y) topLeft = (position.X - 1, position.Y - 1);
        (int X, int Y) top = (position.X, position.Y - 1);
        (int X, int Y) topRight = (position.X + 1, position.Y - 1);
        (int X, int Y) left = (position.X - 1, position.Y);
        (int X, int Y) leftleft = (position.X - 2, position.Y);
        (int X, int Y) right = (position.X + 1, position.Y);
        (int X, int Y) bottomLeft = (position.X - 1, position.Y + 1);
        (int X, int Y) bottom = (position.X, position.Y + 1);
        (int X, int Y) bottomRight = (position.X + 1, position.Y + 1);
        
        return new CellNeighbors(
            TopLeft: IsWithinBounds(playGround.Dimension, topLeft.X, topLeft.Y) ? playGround[topLeft] : SandCellState.Empty,
            Top: IsWithinBounds(playGround.Dimension, top.X, top.Y) ? playGround[top] : SandCellState.Empty,
            TopRight: IsWithinBounds(playGround.Dimension, topRight.X, topRight.Y) ? playGround[topRight] : SandCellState.Empty,
            Left: IsWithinBounds(playGround.Dimension, left.X, left.Y) ? playGround[left] : SandCellState.Empty,
            LeftLeft: IsWithinBounds(playGround.Dimension, left.X - 1, left.Y) ? playGround[leftleft] : SandCellState.Empty,
            Right: IsWithinBounds(playGround.Dimension, right.X, right.Y) ? playGround[right] : SandCellState.Empty,
            BottomLeft: IsWithinBounds(playGround.Dimension, bottomLeft.X, bottomLeft.Y) ? playGround[bottomLeft] : SandCellState.Empty,
            Bottom: IsWithinBounds(playGround.Dimension, bottom.X, bottom.Y) ? playGround[bottom] : SandCellState.Empty,
            BottomRight: IsWithinBounds(playGround.Dimension, bottomRight.X, bottomRight.Y) ? playGround[bottomRight] : SandCellState.Empty
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector dimension, int x, int y )
    {
        return x >= 0 && y >= 0 &&
               x < dimension.X &&
               y < dimension.Y;
    }
    
    private bool IsSand(SandCellState cellState)
    {
        return cellState is SandCellState.Sand or SandCellState.SandDark or SandCellState.SandLight or SandCellState.SandMedium;
    }
}