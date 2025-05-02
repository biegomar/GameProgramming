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
/// <description>Indicates whether the cell has moved.</description>
/// </item>
/// <item>
/// <term>1</term>
/// <description>Indicates whether the cell has NOT moved since the last generation.</description>
/// </item>
/// <item>
/// <term>2</term>
/// <description>Indicates whether the cell has NOT moved since the last TWO generations.</description>
/// </item>
/// <item>
/// <term>3</term>
/// <description>Indicates if the cell is moving to the bottom left.</description>
/// </item>
/// <item>
/// <term>4</term>
/// <description>Indicates if the cell is sliding to the left.</description>
/// </item>
/// <item>
/// <term>5-7</term>
/// <description>Reserved for future use or custom cell behavior.</description>
/// </item>
/// </list>
/// </param>

[StructLayout(LayoutKind.Sequential, Size = 3)]
public record struct Cell(CellType Type, CellColor Color, byte Flags = 0)
{
    public bool IsFlagSet(int bitPosition)
    {
        return (Flags & (1 << bitPosition)) != 0;
    }
    
    public Cell WithFlag(int bitPosition, bool value)
    {
        var newFlags = value 
            ? (byte)(Flags | (1 << bitPosition))         // Setzen
            : (byte)(Flags & ~(1 << bitPosition));        // Zurücksetzen
        
        return this with { Flags = newFlags };
    }
}