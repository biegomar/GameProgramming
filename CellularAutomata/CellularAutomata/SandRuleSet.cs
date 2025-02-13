namespace CellularAutomata;

public class SandRuleSet : IRuleSet<SandCellState>
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
        var localPlayGround = (PlayGround<SandCellState>)playGround;
        var cellState = localPlayGround[position];

        if (cellState == SandCellState.Solid)
        {
            return SandCellState.Solid;
        }

        var cellNeighbors = GetNeighboursState(localPlayGround, position);
        var cellNeighborsFromLeft = GetNeighboursState(localPlayGround, new Vector(position.X - 1, position.Y, 0));
        
        if (cellState == SandCellState.Sand)
        {
            if (
                (cellNeighbors.Bottom == SandCellState.Empty ||
                (cellNeighbors.BottomRight == SandCellState.Empty && cellNeighbors.Right == SandCellState.Empty) ||
                (cellNeighbors.BottomLeft == SandCellState.Empty && cellNeighbors.Left == SandCellState.Empty && cellNeighborsFromLeft.Left == SandCellState.Empty))
                && position.Y < localPlayGround.Dimension.Y - 1
               )
            {
                return SandCellState.Empty;
            }
            
            return SandCellState.Sand;
        }
           
        // Prio 1: grain above me
        if (cellNeighbors.Top == SandCellState.Sand)
        {
            return SandCellState.Sand;
        }

        // Prio 2: grain to the top left, but only if its Prio 1 is blocked.
        if (cellNeighbors is { TopLeft: SandCellState.Sand, Left: SandCellState.Sand or SandCellState.Solid, Top: SandCellState.Empty })
        {
            return SandCellState.Sand;
        }

        // Prio 3: grain to the top right, but only if its Prio 1 and Prio 2 is blocked.
        var cellNeighborsFromRight = GetNeighboursState(localPlayGround, new Vector(position.X + 1, position.Y, 0));
        if (cellNeighbors is { TopRight: SandCellState.Sand, Top: SandCellState.Empty, Right: SandCellState.Sand or SandCellState.Solid }
            && (cellNeighborsFromRight is { Right : SandCellState.Sand or SandCellState.Solid} || (cellNeighborsFromRight.Right == SandCellState.Empty && cellNeighborsFromRight.TopRight != SandCellState.Empty)))
        {
            return SandCellState.Sand; 
        }

        return SandCellState.Empty;
    }

    public SandCellState ApplyRules(IPlayGround<SandCellState> playGround, (int X, int Y, int Z) position)
    {
        return ApplyRules(playGround, new Vector(position.X, position.Y, position.Z));
    }

    public IPlayGround<SandCellState> ApplySpawnRules(IPlayGround<SandCellState> playGround, bool isSpawn)
    {
        var localPlayGround = (PlayGround<SandCellState>)playGround;
        
        if (isSpawn)
        {
            
            var position = new Vector(localPlayGround.Dimension.X / 2, 0, 0);
            var cellNeighbors = GetNeighboursState(localPlayGround, position);

            if (cellNeighbors.Bottom == SandCellState.Empty)
            {
                localPlayGround[position] = SandCellState.Sand;    
            }    
        }
        
        return localPlayGround;
    }

    private CellNeighbors GetNeighboursState(PlayGround<SandCellState> playGround, Vector position)
    {
        var topLeft = new Vector(position.X - 1, position.Y - 1, 0);
        var top = new Vector(position.X, position.Y - 1, 0);
        var topRight = new Vector(position.X + 1, position.Y - 1, 0);
        var left = new Vector(position.X - 1, position.Y, 0);
        var right = new Vector(position.X + 1, position.Y, 0);
        var bottomLeft = new Vector(position.X - 1, position.Y + 1, 0);
        var bottom = new Vector(position.X, position.Y + 1, 0);
        var bottomRight = new Vector(position.X + 1, position.Y + 1, 0);
        
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
}