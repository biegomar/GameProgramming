namespace CellularAutomata;

public class SandRuleSet : IRuleSet<SandCellState>
{
    private struct CellNeighbors
    {
        public SandCellState TopLeft { get; set; }
        public SandCellState Left { get; set; }
        public SandCellState TopMiddle { get; set; }
        public SandCellState Right { get; set; }
        public SandCellState TopRight { get; set; }
        public SandCellState TopRightRight { get; set; }
        public SandCellState RightRight { get; set; }
    }

    
    public SandCellState ApplyRules(PlayGround<SandCellState> playGround, Vector position)
    {
        var cellState = playGround[position];

        if (cellState == SandCellState.Solid)
        {
            return SandCellState.Solid;
        }

        if (cellState == SandCellState.Sand)
        {
            var (left, middle, right) = GetEmptyNeighboursBelow(playGround, position);

            if (!(left is null && middle is null && right is null))
            {
                return SandCellState.Empty;
            }
            
            return SandCellState.Sand;
        }

        var cellNeighbors = GetNeighboursState(playGround, position);
           
        // Prio 1: grain above me
        if (cellNeighbors.TopMiddle == SandCellState.Sand)
        {
            return SandCellState.Sand;
        }

        // Prio 2: grain to the top left, but only if its Prio 1 is blocked.
        if (cellNeighbors is { TopLeft: SandCellState.Sand, Left: SandCellState.Sand or SandCellState.Solid, TopMiddle: SandCellState.Empty })
        {
            return SandCellState.Sand;
        }

        // Prio 3: grain to the top right, but only if its Prio 1 and Prio 2 is blocked.
        if (cellNeighbors is
            {
                TopRight: SandCellState.Sand, 
                Right: SandCellState.Sand or SandCellState.Solid, 
                RightRight: SandCellState.Sand or SandCellState.Solid,
                TopMiddle: SandCellState.Empty
            })
        {
            return SandCellState.Sand;
        }

        return SandCellState.Empty;
    }

    public PlayGround<SandCellState> ApplySpawnRules(PlayGround<SandCellState> playGround)
    {
        var position = new Vector(playGround.Dimension.X / 2, 0, 0);
        var (_, middle,_) = GetEmptyNeighboursBelow(playGround, position);

        if (middle != null)
        {
            playGround[position] = SandCellState.Sand;    
        }
        
        return playGround;
    }

    private (Vector? left, Vector? middle, Vector? right) GetEmptyNeighboursBelow(PlayGround<SandCellState> playGround, Vector position)
    {
        var left = new Vector(position.X - 1, position.Y + 1, 0);
        var middle = new Vector(position.X, position.Y + 1, 0);
        var right = new Vector(position.X + 1, position.Y + 1, 0);

        return (IsWithinBounds(playGround.Dimension, left) && playGround[left] == SandCellState.Empty ? left : null, 
            IsWithinBounds(playGround.Dimension, middle) && playGround[middle] == SandCellState.Empty ? middle : null,
            IsWithinBounds(playGround.Dimension, right) && playGround[right] == SandCellState.Empty ? right : null);
    }
    
    private CellNeighbors GetNeighboursState(PlayGround<SandCellState> playGround, Vector position)
    {
        var topLeft = new Vector(position.X - 1, position.Y - 1, 0);
        var left = new Vector(position.X - 1, position.Y, 0);
        var topMiddle = new Vector(position.X, position.Y - 1, 0);
        var right = new Vector(position.X + 1, position.Y, 0);
        var topRight = new Vector(position.X + 1, position.Y - 1, 0);
        var rightRight = new Vector(position.X + 2, position.Y, 0);
        var topRightRight = new Vector(position.X + 2, position.Y - 1, 0);
        
        var result = new CellNeighbors
        {
            TopLeft = IsWithinBounds(playGround.Dimension, topLeft) ? playGround[topLeft] : SandCellState.Empty,
            Left = IsWithinBounds(playGround.Dimension, left) ? playGround[left] : SandCellState.Empty,
            TopMiddle = IsWithinBounds(playGround.Dimension, topMiddle) ? playGround[topMiddle] : SandCellState.Empty,
            TopRight = IsWithinBounds(playGround.Dimension, topRight) ? playGround[topRight] : SandCellState.Empty,
            Right = IsWithinBounds(playGround.Dimension, right) ? playGround[right] : SandCellState.Empty,
            RightRight = IsWithinBounds(playGround.Dimension, rightRight) ? playGround[rightRight] : SandCellState.Empty,
            TopRightRight = IsWithinBounds(playGround.Dimension, topRightRight) ? playGround[topRightRight] : SandCellState.Empty
        };

        return result;
    }
    
    private bool IsWithinBounds(Vector dimension, Vector position)
    {
        return position.X >= 0 && position.Y >= 0 &&
               position.X < dimension.X &&
               position.Y < dimension.Y;
    }
}