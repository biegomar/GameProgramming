using System.Runtime.CompilerServices;
using CellularAutomata.Cells;

namespace CellularAutomata.PlayGrounds;

public sealed class PlayGround
{
    private readonly Cell[] cells;
    
    public Vector Dimension { get; }
    
    private readonly uint dimensionX;
    private readonly uint dimensionY;
    
    public PlayGround(Vector dimension, Func<int, int, Cell>? cellFactory = null)
    {
        Dimension = dimension;
        dimensionX = (uint)dimension.X;
        dimensionY = (uint)dimension.Y;
        
        cells = new Cell[dimensionX * dimensionY];
        
        Initialize(cellFactory);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Cell GetCell(Vector position)
    {
        return cells[this.GetIndex(position)]; 
    }

    public void SetCell(Vector position, Cell cell)
    {
        if (IsWithinBounds(position))
        {
            cells[this.GetIndex(position)] = cell;    
        }
    }
    
    private void Initialize(Func<int, int, Cell>? cellFactory = null)
    {
        const CellColor cellBrightness = CellColor.Empty;
        const CellType cellType = CellType.Empty;

        for (ushort x = 0; x < this.dimensionX; x++)
        {
            for (ushort y = 0; y < this.dimensionY; y++)
            {
                this.cells[this.GetIndex(new Vector(x, y))] = cellFactory != null 
                    ? cellFactory(x, y) 
                    : new Cell(cellType, cellBrightness);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private uint GetIndex(Vector position)
    {
        return (uint)position.Y * dimensionX + (uint)position.X;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector position)
    {
        return (uint)position.X < Dimension.X && (uint)position.Y < Dimension.Y;
    }
}