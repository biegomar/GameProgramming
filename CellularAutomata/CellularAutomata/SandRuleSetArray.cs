using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CellularAutomata;

public sealed class SandRuleSetArray(Vector dimension) : IRuleSet
{
    private const CellState Solid = CellState.Solid;
    private const CellState Empty = CellState.Empty;
    private readonly Random random = new ();
    
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
    
    public IPlayGround ApplySpawnRules(IPlayGround playGround, Vector spawnPosition)
    {
        var startX = spawnPosition.X - 5;
        var endX = spawnPosition.X + 4;
        
        var startY = spawnPosition.Y - 5;
        var endY = spawnPosition.Y + 4;

        for (var x = startX; x <= endX; x++)
        {
            for (var y = startY; y <= endY; y++)
            {
                var newPos = new Vector(x, y);
                if (IsWithinBounds(newPos) && playGround[newPos] == Empty)
                {
                    playGround[newPos] = GetRandomSandCellState();
                }
            }    
        }
        
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
            TopLeft: IsWithinBounds(topLeft) ? playGround[topLeft] : Empty,
            Top: IsWithinBounds(top) ? playGround[top] : Empty,
            TopRight: IsWithinBounds(topRight) ? playGround[topRight] : Empty,
            Left: IsWithinBounds(left) ? playGround[left] : Empty,
            LeftLeft: IsWithinBounds(leftleft) ? playGround[leftleft] : Empty,
            Right: IsWithinBounds(right) ? playGround[right] : Empty,
            BottomLeft: IsWithinBounds(bottomLeft) ? playGround[bottomLeft] : Empty,
            Bottom: IsWithinBounds(bottom) ? playGround[bottom] : Empty,
            BottomRight: IsWithinBounds(bottomRight) ? playGround[bottomRight] : Empty
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector position )
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
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CellState GetRandomSandCellState()
    {
        var randomValue = random.Next(2, 6);

        return (CellState)randomValue;

    }
}