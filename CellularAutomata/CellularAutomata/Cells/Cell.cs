using System.Runtime.InteropServices;

namespace CellularAutomata.Cells;

[StructLayout(LayoutKind.Sequential, Size = 3)]
public record struct Cell(CellType Type, CellColor Color, byte State = 0);