using System.Runtime.InteropServices;

namespace CellularAutomata.Cells;

/// <summary>
/// A cell for the automata.
/// Represents a single cell with type, color, and configurable flags.
/// </summary>
/// <param name="Type">The cell type.</param>
/// <param name="Color">The actual color of the cell.</param>
/// <param name="Flags">
/// A compact, 8-bit representation for various cell states or features.
/// <list type="table">
/// <listheader>
/// <term>Bit</term>
/// <description>Description</description>
/// </listheader>
/// <item>
/// <term>0</term>
/// <description>Indicates whether the cell has been moved.</description>
/// </item>
/// <item>
/// <term>1</term>
/// <description>Indicates if the cell is sliding to the left.</description>
/// </item>
/// <item>
/// <term>2</term>
/// <description>Indicates if the cell is moving in the left direction.</description>
/// </item>
/// <item>
/// <term>3</term>
/// <description>Indicates if the cell is moving in the right direction.</description>
/// </item>
/// <item>
/// <term>4-7</term>
/// <description>A 4-bit counter.</description>
/// </item>
/// </list>
/// </param>

[StructLayout(LayoutKind.Sequential, Size = 3)]
public readonly record struct Cell(CellType Type, CellColor Color, byte Flags = 0)
{
    private const int CounterMask = 0b1111_0000;

    public bool IsFlagSet(int bitPosition)
    {
        return (Flags & (1 << bitPosition)) != 0;
    }
    
    public Cell WithFlag(int bitPosition, bool value)
    {
        var newFlags = value 
            ? (byte)(Flags | (1 << bitPosition))
            : (byte)(Flags & ~(1 << bitPosition));
        
        return this with { Flags = newFlags };
    }
    
    /// <summary>
    /// Reads the counter from bits 4-7.
    /// </summary>
    public int GetCounter()
    {
        return (Flags & CounterMask) >> 4; // Verschiebt Bits 4-7 nach rechts
    }
    
    /// <summary>
    /// Sets the counter in bits 4-7.
    /// </summary>
    public Cell WithCounter(int counterValue)
    {
        if (counterValue < 0 || counterValue > 15)
        {
            throw new ArgumentOutOfRangeException(nameof(counterValue), "Value must be between 0 and 15.");
        }
        
        var newFlags = (byte)((Flags & ~CounterMask) | (counterValue << 4)); 
        return this with { Flags = newFlags };
    }

    /// <summary>
    /// Increments the counter in bits 4-7 by 1, if it does not exceed 15.
    /// </summary>
    public Cell WithCounterIncrement()
    {
        int currentCounter = GetCounter();
        return currentCounter < 15 ? WithCounter(currentCounter + 1) : this; // Counter is already at 15, so do nothing.
    }

    /// <summary>
    /// Decrements the counter in bits 4-7 by 1, if it is greater than 0.
    /// </summary>
    public Cell WithCounterDecrement()
    {
        int currentCounter = GetCounter();
        return currentCounter > 0 ? WithCounter(currentCounter - 1) : this; // Counter is already at 0, so do nothing.   
    }

}