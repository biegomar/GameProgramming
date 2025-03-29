using System.Runtime.InteropServices;

namespace CellularAutomata;

[StructLayout(LayoutKind.Sequential, Size = 2)]
public record struct Cell(CellState State, bool HasMoved = false);