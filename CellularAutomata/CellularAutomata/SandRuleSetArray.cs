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
        
        // First look at a cell with state - so we push the grain.

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
        
        // We are sure. That cell is empty. Now we pull the grain.
        
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
        var cellNeighborsFromRight = GetNeighboursState(playGround, new Vector(position.X + 1, position.Y));
        
        var canPullFromRight = IsSand(cellNeighbors.TopRight) && IsSandOrSolid(cellNeighbors.Right) && cellNeighbors.Top == Empty;
        var isStrongCriteriaToPullFromRight = IsSandOrSolid(cellNeighborsFromRight.Right) ||
                                              (cellNeighborsFromRight.Right == Empty && IsSand(cellNeighborsFromRight.TopRight));

        if (IsStrongPullCriteriaNeeded())
        {
            if (canPullFromRight && isStrongCriteriaToPullFromRight)
            {
                //RuleCounter["Prio3"]++;
                return cellNeighbors.TopRight; 
            }    
        }
        else
        {
            if (canPullFromRight)
            {
                //RuleCounter["Prio3"]++;
                return cellNeighbors.TopRight; 
            }
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
        return new CellNeighbors(
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
    private bool IsStrongPullCriteriaNeeded()
    {
        var result = random.Next(0, 2) == 0;
        return result;
    }
}