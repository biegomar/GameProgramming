using System.Runtime.InteropServices;

namespace CellularAutomata.Cells;

[StructLayout(LayoutKind.Sequential, Size = 12)]
public record struct Cell(CellType Type, CellBrightness Brightness, bool IsOccupied=false);