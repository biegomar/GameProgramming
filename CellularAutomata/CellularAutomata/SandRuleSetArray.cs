using System.Runtime.CompilerServices;

namespace CellularAutomata;

public sealed class SandRuleSetArray : IRuleSet<SandCellState>
{
    private static readonly (int DX, int DY)[] NeighborOffsets = 
    {
        (-1, -1), (0, -1), (1, -1),
        ( -1, 0),          ( 1, 0),
        ( -1, 1), ( 0, 1), ( 1, 1),
    };
    
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
        return this.ApplyRules(playGround, (position.X, position.Y));
    }
    
    public SandCellState ApplyRules(IPlayGround<SandCellState> playGround, (int X, int Y) position)
    {
        var cellState = playGround[position];

        if (cellState == SandCellState.Solid)
        {
            return SandCellState.Solid;
        }

        var cellNeighbors = GetNeighboursState(playGround, position);
        var cellNeighborsFromLeft = GetNeighboursState(playGround, (position.X - 1, position.Y));
        
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
        var cellNeighborsFromRight = GetNeighboursState(playGround, (position.X + 1, position.Y));
        if (cellNeighbors is { TopRight: SandCellState.Sand, Top: SandCellState.Empty, Right: SandCellState.Sand or SandCellState.Solid }
            && (cellNeighborsFromRight is { Right : SandCellState.Sand or SandCellState.Solid} || (cellNeighborsFromRight.Right == SandCellState.Empty && cellNeighborsFromRight.TopRight != SandCellState.Empty)))
        {
            return SandCellState.Sand; 
        }

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
        (int X, int Y) right = (position.X + 1, position.Y);
        (int X, int Y) bottomLeft = (position.X - 1, position.Y + 1);
        (int X, int Y) bottom = (position.X, position.Y + 1);
        (int X, int Y) bottomRight = (position.X + 1, position.Y + 1);
        
        return new CellNeighbors(
            TopLeft: IsWithinBounds(playGround.Dimension, topLeft.X, topLeft.Y) ? playGround[topLeft] : SandCellState.Empty,
            Top: IsWithinBounds(playGround.Dimension, top.X, top.Y) ? playGround[top] : SandCellState.Empty,
            TopRight: IsWithinBounds(playGround.Dimension, topRight.X, topRight.Y) ? playGround[topRight] : SandCellState.Empty,
            Left: IsWithinBounds(playGround.Dimension, left.X, left.Y) ? playGround[left] : SandCellState.Empty,
            Right: IsWithinBounds(playGround.Dimension, right.X, right.Y) ? playGround[right] : SandCellState.Empty,
            BottomLeft: IsWithinBounds(playGround.Dimension, bottomLeft.X, bottomLeft.Y) ? playGround[bottomLeft] : SandCellState.Empty,
            Bottom: IsWithinBounds(playGround.Dimension, bottom.X, bottom.Y) ? playGround[bottom] : SandCellState.Empty,
            BottomRight: IsWithinBounds(playGround.Dimension, bottomRight.X, bottomRight.Y) ? playGround[bottomRight] : SandCellState.Empty
        );
        
        // var neighbors = new SandCellState[8];
        // int index = 0;
        //
        // foreach (var (dx, dy) in NeighborOffsets)
        // {
        //     var nx = position.X + dx;
        //     var ny = position.Y + dy;
        //     
        //     neighbors[index++] = IsWithinBounds(playGround.Dimension, nx, ny)
        //         ? playGround[(nx, ny)]
        //         : SandCellState.Empty; 
        // }
        //
        // return new CellNeighbors(
        //     TopLeft: neighbors[0],
        //     Top: neighbors[1],
        //     TopRight: neighbors[2],
        //     Left: neighbors[3],
        //     Right: neighbors[4],
        //     BottomLeft: neighbors[5],
        //     Bottom: neighbors[6],
        //     BottomRight: neighbors[7]
        // );
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector dimension, int x, int y )
    {
        return x >= 0 && y >= 0 &&
               x < dimension.X &&
               y < dimension.Y;
    }
    
}