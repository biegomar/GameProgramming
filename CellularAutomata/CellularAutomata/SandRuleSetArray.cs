namespace CellularAutomata;

public class SandRuleSetArray : IRuleSet<SandCellState>
{
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


    public SandCellState ApplyRules(PlayGround<SandCellState> playGround, Vector position)
    {
        return this.ApplyRules((PlayGroundArray<SandCellState>)playGround, ((int)position.X, (int)position.Y, (int)position.Z));
    }

    public SandCellState ApplyRules(PlayGroundArray<SandCellState> playGround, (int X, int Y, int Z) position)
    {
        var cellState = playGround[position];

        if (cellState == SandCellState.Solid)
        {
            return SandCellState.Solid;
        }

        var cellNeighbors = GetNeighboursState(playGround, position);
        var cellNeighborsFromLeft = GetNeighboursState(playGround, (position.X - 1, position.Y, 0));
        
        if (cellState == SandCellState.Sand)
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
        var cellNeighborsFromRight = GetNeighboursState(playGround, (position.X + 1, position.Y, 0));
        if (cellNeighbors is { TopRight: SandCellState.Sand, Top: SandCellState.Empty, Right: SandCellState.Sand or SandCellState.Solid }
            && (cellNeighborsFromRight is { Right : SandCellState.Sand or SandCellState.Solid} || (cellNeighborsFromRight.Right == SandCellState.Empty && cellNeighborsFromRight.TopRight != SandCellState.Empty)))
        {
            return SandCellState.Sand; 
        }

        return SandCellState.Empty;
    }

    public PlayGround<SandCellState> ApplySpawnRules(PlayGround<SandCellState> playGround, bool isSpawn)
    {
        return this.ApplySpawnRules((PlayGroundArray<SandCellState>)playGround, isSpawn);
    }

    public PlayGroundArray<SandCellState> ApplySpawnRules(PlayGroundArray<SandCellState> playGround, bool isSpawn)
    {
        if (isSpawn)
        {
            var position = ((int)playGround.Dimension.X / 2, 0, 0);
            var cellNeighbors = GetNeighboursState(playGround, position);

            if (cellNeighbors.Bottom == SandCellState.Empty)
            {
                playGround[position] = SandCellState.Sand;    
            }    
        }
        
        return playGround;
    }

    private CellNeighbors GetNeighboursState(PlayGroundArray<SandCellState> playGround, (int X, int Y, int Z) position)
    {
        var topLeft = (position.X - 1, position.Y - 1, 0);
        var top = (position.X, position.Y - 1, 0);
        var topRight = (position.X + 1, position.Y - 1, 0);
        var left = (position.X - 1, position.Y, 0);
        var right = (position.X + 1, position.Y, 0);
        var bottomLeft = (position.X - 1, position.Y + 1, 0);
        var bottom = (position.X, position.Y + 1, 0);
        var bottomRight = (position.X + 1, position.Y + 1, 0);
        
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

    
    private bool IsWithinBounds(Vector dimension, (int X, int Y, int Z) position)
    {
        return position.X >= 0 && position.Y >= 0 &&
               position.X < dimension.X &&
               position.Y < dimension.Y;
    }
}