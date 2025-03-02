using System.Runtime.CompilerServices;

namespace CellularAutomata;

public sealed class PlayGroundArray<T> : IPlayGround<T>
{
    public Cell<T>[] Cells { get; }
    
    public Vector Dimension { get; }

    public PlayGroundArray(Vector dimension)
    {
        Dimension = dimension;
        Cells = new Cell<T>[dimension.X * dimension.Y];
        
        Initialize();
    }
    
    public T this[Vector position]
    {
        get => Cells[this.GetIndex(position.X, position.Y)].State;
        set => Cells[this.GetIndex(position.X, position.Y)].State = value;
    }

    public T this[(int x, int y) position]
    {
        get => Cells[this.GetIndex(position.x, position.y)].State;
        set => Cells[this.GetIndex(position.x, position.y)].State = value;
    }
    
    private void Initialize()
    {
        var defaultState = default(T)!;
        
        for (var x = 0; x < this.Dimension.X; x++)
        {
            for (var y = 0; y < this.Dimension.Y; y++)
            {
                this.Cells[this.GetIndex(x, y)] = new Cell<T>(defaultState, x, y);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetIndex(int x, int y)
    {
        return x * this.Dimension.Y + y;
    }
}