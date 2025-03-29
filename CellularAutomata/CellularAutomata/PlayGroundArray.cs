using System.Runtime.CompilerServices;

namespace CellularAutomata;

public sealed class PlayGroundArray : IPlayGround
{
    public Cell[] Cells { get; }
    
    public Vector Dimension { get; }
    
    private readonly int dimensionX;
    private readonly int dimensionY;


    public PlayGroundArray(Vector dimension, Func<int, int, Cell>? cellFactory = null)
    {
        Dimension = dimension;
        dimensionX = dimension.X;
        dimensionY = dimension.Y;
        
        Cells = new Cell[dimensionX * dimensionY];
        
        Initialize(cellFactory);
    }
    
    public CellState this[Vector position]
    {
        get => Cells[this.GetIndex(position.X, position.Y)].State;
        set => Cells[this.GetIndex(position.X, position.Y)].State = value;
    }
    
    private void Initialize(Func<int, int, Cell>? cellFactory = null)
    {
        const CellState defaultState = CellState.Empty;

        for (ushort x = 0; x < this.dimensionX; x++)
        {
            for (ushort y = 0; y < this.dimensionY; y++)
            {
                this.Cells[this.GetIndex(x, y)] = cellFactory != null 
                    ? cellFactory(x, y) 
                    : new Cell(defaultState);

            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetIndex(int x, int y)
    {
        return x * dimensionY + y;
    }
}