using System.Runtime.CompilerServices;
using CellularAutomata.Cells;

namespace CellularAutomata.PlayGrounds;

public sealed class PlayGround
{
    public Cell[] Cells { get; }
    
    public Vector Dimension { get; }
    
    private readonly int dimensionX;
    private readonly int dimensionY;
    
    public PlayGround(Vector dimension, Func<int, int, Cell>? cellFactory = null)
    {
        Dimension = dimension;
        dimensionX = dimension.X;
        dimensionY = dimension.Y;
        
        Cells = new Cell[dimensionX * dimensionY];
        
        Initialize(cellFactory);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Cell GetCell(Vector position)
    {
        return Cells[this.GetIndex(position)];
    }

    public void SetCell(Vector position, Cell cell)
    {
        Cells[this.GetIndex(position)] = cell;
    }

    public CellType GetCellType(Vector position)
    {
        return Cells[this.GetIndex(position)].Type;
    }

    public void SetCellType(Vector position, CellType cellType)
    {
        Cells[this.GetIndex(position)].Type = cellType;
    }

    private void Initialize(Func<int, int, Cell>? cellFactory = null)
    {
        const CellBrightness cellBrightness = CellBrightness.Empty;
        const CellType cellType = CellType.Empty;

        for (ushort x = 0; x < this.dimensionX; x++)
        {
            for (ushort y = 0; y < this.dimensionY; y++)
            {
                this.Cells[this.GetIndex(new Vector(x, y))] = cellFactory != null 
                    ? cellFactory(x, y) 
                    : new Cell(cellType, cellBrightness);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetIndex(Vector position)
    {
        return position.Y * dimensionX + position.X;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector position )
    {
        return (uint)position.X < (uint)Dimension.X && (uint)position.Y < (uint)Dimension.Y;
    }
}