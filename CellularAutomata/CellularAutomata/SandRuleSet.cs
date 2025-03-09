namespace CellularAutomata;

public sealed class SandRuleSet : IRuleSet<SandCellState>
{
    public IDictionary<string, uint> RuleCounter { get; init; } = new Dictionary<string, uint>();
    
    private record CellNeighbors(
        SandCellState TopLeft,
        SandCellState Top,
        SandCellState TopRight,
        SandCellState Left,
        SandCellState Right,
        SandCellState BottomLeft,
        SandCellState Bottom,
        SandCellState BottomRight
    );
    
    public SandCellState ApplyRules(IPlayGround<SandCellState> playGround, Vector position)
    {
        var cellState = playGround[position];

        if (cellState == SandCellState.Solid)
        {
            return SandCellState.Solid;
        }

        var cellNeighbors = GetNeighboursState(playGround, position);
        var cellNeighborsFromLeft = GetNeighboursState(playGround, new Vector(position.X - 1, position.Y));
        
        if (IsSand(cellState))
        {
            if (
                (cellNeighbors.Bottom == SandCellState.Empty ||
                (cellNeighbors.BottomRight == SandCellState.Empty && cellNeighbors.Right == SandCellState.Empty) ||
                (cellNeighbors.BottomLeft == SandCellState.Empty && cellNeighbors.Left == SandCellState.Empty && cellNeighborsFromLeft.Left == SandCellState.Empty))
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
            return cellNeighbors.Top;
        }

        // Prio 2: grain to the top left, but only if its Prio 1 is blocked.
        if (IsSand(cellNeighbors.TopLeft) && (IsSand(cellNeighbors.Left) || cellNeighbors.Left == SandCellState.Solid) &&
            cellNeighbors.Top == SandCellState.Empty)
        {
            return cellNeighbors.TopLeft;
        }

        // Prio 3: grain to the top right, but only if its Prio 1 and Prio 2 is blocked.
        var cellNeighborsFromRight = GetNeighboursState(playGround, new Vector(position.X + 1, position.Y));
        if (IsSand(cellNeighbors.TopRight) &&
            cellNeighbors.Top == SandCellState.Empty &&
            (IsSand(cellNeighbors.Right) || cellNeighbors.Right == SandCellState.Solid) &&
            (IsSand(cellNeighborsFromRight.Right) || cellNeighborsFromRight.Right == SandCellState.Solid || 
             (cellNeighborsFromRight.Right == SandCellState.Empty && cellNeighborsFromRight.TopRight != SandCellState.Empty)))
        {
            return cellNeighbors.TopRight; 
        }

        return SandCellState.Empty;
    }

    public SandCellState ApplyRules(IPlayGround<SandCellState> playGround, (int X, int Y) position)
    {
        return ApplyRules(playGround, new Vector(position.X, position.Y));
    }

    public IPlayGround<SandCellState> ApplySpawnRules(IPlayGround<SandCellState> playGround, bool isSpawn)
    {
        var localPlayGround = (PlayGround<SandCellState>)playGround;
        
        if (isSpawn)
        {
            
            var position = new Vector(localPlayGround.Dimension.X / 2, 0);
            var cellNeighbors = GetNeighboursState(localPlayGround, position);

            if (cellNeighbors.Bottom == SandCellState.Empty)
            {
                localPlayGround[position] = SandCellState.Sand;    
            }    
        }
        
        return localPlayGround;
    }

    private CellNeighbors GetNeighboursState(IPlayGround<SandCellState> playGround, Vector position)
    {
        var topLeft = new Vector(position.X - 1, position.Y - 1);
        var top = new Vector(position.X, position.Y - 1);
        var topRight = new Vector(position.X + 1, position.Y - 1);
        var left = new Vector(position.X - 1, position.Y);
        var right = new Vector(position.X + 1, position.Y);
        var bottomLeft = new Vector(position.X - 1, position.Y + 1);
        var bottom = new Vector(position.X, position.Y + 1);
        var bottomRight = new Vector(position.X + 1, position.Y + 1);
        
        return new CellNeighbors(
            TopLeft: IsWithinBounds(playGround.Dimension, topLeft) ? playGround[topLeft] : SandCellState.Empty,
            Top: IsWithinBounds(playGround.Dimension, top) ? playGround[top] : SandCellState.Empty,
            TopRight: IsWithinBounds(playGround.Dimension, topRight) ? playGround[topRight] : SandCellState.Empty,
            Left: IsWithinBounds(playGround.Dimension, left) ? playGround[left] : SandCellState.Empty,
            Right: IsWithinBounds(playGround.Dimension, right) ? playGround[right] : SandCellState.Empty,
            BottomLeft: IsWithinBounds(playGround.Dimension, bottomLeft) ? playGround[bottomLeft] : SandCellState.Empty,
            Bottom: IsWithinBounds(playGround.Dimension, bottom) ? playGround[bottom] : SandCellState.Empty,
            BottomRight: IsWithinBounds(playGround.Dimension, bottomRight) ? playGround[bottomRight] : SandCellState.Empty
        );
    }
    
    private bool IsWithinBounds(Vector dimension, Vector position)
    {
        return position.X >= 0 && position.Y >= 0 &&
               position.X < dimension.X &&
               position.Y < dimension.Y;
    }
    
    private bool IsSand(SandCellState cellState)
    {
        return cellState is SandCellState.Sand or SandCellState.SandDark or SandCellState.SandLight or SandCellState.SandMedium;
    }
}