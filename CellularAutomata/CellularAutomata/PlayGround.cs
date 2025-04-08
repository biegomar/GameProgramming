using System.Collections;
using System.Runtime.CompilerServices;

namespace CellularAutomata;

public sealed class PlayGround : IPlayGround
{
    public Cell[] Cells { get; }
    private readonly BitArray processedRightCells;
    private readonly BitArray processedLeftCells;

    
    public Vector Dimension { get; }
    
    private readonly int dimensionX;
    private readonly int dimensionY;
    
    public PlayGround(Vector dimension, Func<int, int, Cell>? cellFactory = null)
    {
        Dimension = dimension;
        dimensionX = dimension.X;
        dimensionY = dimension.Y;
        
        Cells = new Cell[dimensionX * dimensionY];
        processedRightCells = new BitArray(dimensionX * dimensionY);
        processedLeftCells = new BitArray(dimensionX * dimensionY);
        
        Initialize(cellFactory);
    }
    
    public CellState this[Vector position]
    {
        get => Cells[this.GetIndex(position)].State;
        set => Cells[this.GetIndex(position)].State = value;
    }

    public void MarkAsProcessedRight(Vector position)
    {
        processedRightCells.Set(GetIndex(position), true);
    }

    public void MarkAsProcessedLeft(Vector position)
    {
        processedLeftCells.Set(GetIndex(position), true);
    }

    public void ResetMovedCells()
    {
        processedRightCells.SetAll(false);
        processedLeftCells.SetAll(false);
    }

    public bool IsProcessedRight(Vector position)
    {
        return IsWithinBounds(position) && processedRightCells.Get(GetIndex(position));
    }

    public bool IsProcessedLeft(Vector position)
    {
        return IsWithinBounds(position) && processedLeftCells.Get(GetIndex(position));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Cell GetCell(Vector position)
    {
        return Cells[this.GetIndex(position)];
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
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector position )
    {
        return (uint)position.X < (uint)Dimension.X && (uint)position.Y < (uint)Dimension.Y;
    }
}