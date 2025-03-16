using System.Runtime.CompilerServices;

namespace CellularAutomata;

public sealed class PlayGroundArray<T> : IPlayGround<T>
{
    public Cell<T>[] Cells { get; }
    
    public Vector Dimension { get; }
    
    private readonly int dimensionX;
    private readonly int dimensionY;


    public PlayGroundArray(Vector dimension, Func<int, int, Cell<T>>? cellFactory = null)
    {
        Dimension = dimension;
        dimensionX = dimension.X;
        dimensionY = dimension.Y;
        
        Cells = new Cell<T>[dimensionX * dimensionY];
        
        Initialize(cellFactory);
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
    
    private void Initialize(Func<int, int, Cell<T>>? cellFactory = null)
    {
        var defaultState = default(T)!;
        
        for (ushort x = 0; x < this.dimensionX; x++)
        {
            for (ushort y = 0; y < this.dimensionY; y++)
            {
                this.Cells[this.GetIndex(x, y)] = cellFactory != null 
                    ? cellFactory(x, y) 
                    : new Cell<T>(defaultState);

            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetIndex(int x, int y)
    {
        return x * dimensionY + y;
    }
}