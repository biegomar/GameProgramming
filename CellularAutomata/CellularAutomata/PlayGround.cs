using System.Collections;
using System.Runtime.CompilerServices;
using CellularAutomata.Cells;
using CellularAutomata.Interfaces;

namespace CellularAutomata;

public sealed class PlayGround : IPlayGround
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
    
    public CellBrightness this[Vector position]
    {
        get => Cells[this.GetIndex(position)].Brightness;
        set => Cells[this.GetIndex(position)].Brightness = value;
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