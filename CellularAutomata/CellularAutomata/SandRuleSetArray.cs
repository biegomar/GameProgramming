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

        var cellNeighbors = GetNeighboursState(playGround, position.X, position.Y);
        
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
        if (IsSand(cellNeighbors.TopLeft) && IsSandOrSolid(cellNeighbors.Left) && cellNeighbors.Top == Empty)
        {
            //RuleCounter["Prio2"]++;
            return cellNeighbors.TopLeft;
        }

        // Prio 3: grain to the top right, but only if its Prio 1 and Prio 2 is blocked.
        var cellNeighborsFromRight = GetNeighboursState(playGround, position.X + 1, position.Y);
        if (IsSand(cellNeighbors.TopRight) && IsSandOrSolid(cellNeighbors.Right) && cellNeighbors.Top == Empty &&
            (IsSandOrSolid(cellNeighborsFromRight.Right) || 
             (cellNeighborsFromRight.Right == Empty && cellNeighborsFromRight.TopRight != Empty)))
        {
            //RuleCounter["Prio3"]++;
            return cellNeighbors.TopRight; 
        }

        //RuleCounter["Empty"]++;
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
                if (IsWithinBounds(x, y) && playGround[x, y] == Empty)
                {
                    if (random.NextDouble() < probability)
                    {
                        playGround[x, y] = GetRandomSandCellState();   
                    }
                }
            }    
        }
        
        return playGround;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CellNeighbors GetNeighboursState(IPlayGround playGround, int x, int y)
    {
        //var topLeft = new Vector(x - 1, y - 1);
        //var top = new Vector(x, y - 1);
        //var topRight = new Vector(x + 1, y - 1);
        //var left = new Vector(x - 1, y);
        //var leftleft = new Vector(x - 2, y);
        //var right = new Vector(x + 1, y);
        //var bottomLeft = new Vector(x - 1, y + 1);
        //var bottom = new Vector(x, y + 1);
        //var bottomRight = new Vector(x + 1, y + 1);
        
        return new CellNeighbors(
            TopLeft: IsWithinBounds(x - 1, y - 1) ? playGround[x - 1, y - 1] : Solid,
            Top: IsWithinBounds(x, y - 1) ? playGround[x, y - 1] : Solid,
            TopRight: IsWithinBounds(x + 1, y - 1) ? playGround[x + 1, y - 1] : Solid,
            Left: IsWithinBounds(x - 1, y) ? playGround[x - 1, y] : Solid,
            LeftLeft: IsWithinBounds(x - 2, y) ? playGround[x - 2, y] : Solid,
            Right: IsWithinBounds(x + 1, y) ? playGround[x + 1, y] : Solid,
            BottomLeft: IsWithinBounds(x - 1, y + 1) ? playGround[x - 1, y + 1] : Solid,
            Bottom: IsWithinBounds(x, y + 1) ? playGround[x, y + 1] : Solid,
            BottomRight: IsWithinBounds(x + 1, y + 1) ? playGround[x + 1, y + 1] : Solid
        );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(int x, int y)
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
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CellState GetRandomSandCellState()
    {
        var randomValue = random.Next(2, 6);

        return (CellState)randomValue;

    }
}