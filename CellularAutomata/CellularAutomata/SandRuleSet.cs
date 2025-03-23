using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CellularAutomata;

public sealed class SandRuleSet(Vector dimension) : IRuleSet
{
    private const CellState Solid = CellState.Solid;
    private const CellState Empty = CellState.Empty;
    private readonly Random random = new ();

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

        if (cellState == Solid)
        {
            return Solid;
        }

        var cellNeighbors = GetNeighboursState(playGround, position);
        
        if (IsSand(cellState))
        {
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
           
        // Prio 1: grain above me
        if (IsSand(cellNeighbors.Top))
        {
            return cellNeighbors.Top;
        }

        // Prio 2: grain to the top left, but only if its Prio 1 is blocked.
        if (IsSand(cellNeighbors.TopLeft) && (IsSand(cellNeighbors.Left) || cellNeighbors.Left == Solid) &&
            cellNeighbors.Top == Empty)
        {
            return cellNeighbors.TopLeft;
        }

        // Prio 3: grain to the top right, but only if its Prio 1 and Prio 2 is blocked.
        var cellNeighborsFromRight = GetNeighboursState(playGround, new Vector(position.X + 1, position.Y));
        if (IsSand(cellNeighbors.TopRight) &&
            cellNeighbors.Top == Empty &&
            (IsSand(cellNeighbors.Right) || cellNeighbors.Right == Solid) &&
            (IsSand(cellNeighborsFromRight.Right) || cellNeighborsFromRight.Right == Solid || 
             (cellNeighborsFromRight.Right == Empty && cellNeighborsFromRight.TopRight != Empty)))
        {
            return cellNeighbors.TopRight; 
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
                var newPos = new Vector(x, y);
                if (IsWithinBounds(newPos) && playGround[newPos] == Empty)
                {
                    if (random.NextDouble() < probability)
                    {
                        playGround[newPos] = GetRandomSandCellState();   
                    }
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
            TopLeft: IsWithinBounds(topLeft) ? playGround[topLeft] : Solid,
            Top: IsWithinBounds(top) ? playGround[top] : Solid,
            TopRight: IsWithinBounds(topRight) ? playGround[topRight] : Solid,
            Left: IsWithinBounds(left) ? playGround[left] : Solid,
            LeftLeft: IsWithinBounds(leftleft) ? playGround[leftleft] : Solid,
            Right: IsWithinBounds(right) ? playGround[right] : Solid,
            BottomLeft: IsWithinBounds(bottomLeft) ? playGround[bottomLeft] : Solid,
            Bottom: IsWithinBounds(bottom) ? playGround[bottom] : Solid,
            BottomRight: IsWithinBounds(bottomRight) ? playGround[bottomRight] : Solid
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
    private CellState GetRandomSandCellState()
    {
        var randomValue = random.Next(2, 6);

        return (CellState)randomValue;

    }
}