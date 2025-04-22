using System.Runtime.CompilerServices;

namespace CellularAutomata.PlayGrounds;

public class SimplePlayGround
{
    private readonly bool[] cells;
    public Vector Dimension { get; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GetState(Vector position)
    {
        return cells[this.GetIndex(position)];
    }

    public void SetState(Vector position, bool state)
    {
        cells[this.GetIndex(position)] = state;
    }
    
    public SimplePlayGround(Vector dimension)
    {
        this.Dimension = dimension;
        
        cells = new bool[dimension.X * dimension.Y];
    }
    
    public SimplePlayGround(SimplePlayGround other)
    {
        this.Dimension = other.Dimension;
        
        this.cells = (bool[])other.cells.Clone(); 
    }

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetIndex(Vector position)
    {
        return position.Y * Dimension.X + position.X;
    }
}