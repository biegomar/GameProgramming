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
        get => Cells[this.GetIndex(position)].State;
        set => Cells[this.GetIndex(position)].State = value;
    }

    public void SetCellToMoved(Vector position)
    {
        Cells[this.GetIndex(position)].HasMoved = true;
    }

    public void ClearCellToNotMoved(Vector position)
    {
        Cells[this.GetIndex(position)].HasMoved = false;
    }

    public bool HasMoved(Vector position)
    {
        return Cells[this.GetIndex(position)].HasMoved;
    }

    private void Initialize(Func<int, int, Cell>? cellFactory = null)
    {
        const CellState defaultState = CellState.Empty;

        for (ushort x = 0; x < this.dimensionX; x++)
        {
            for (ushort y = 0; y < this.dimensionY; y++)
            {
                this.Cells[this.GetIndex(new Vector(x, y))] = cellFactory != null 
                    ? cellFactory(x, y) 
                    : new Cell(defaultState);

            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetIndex(Vector position)
    {
        return position.X * dimensionY + position.Y;
    }
}