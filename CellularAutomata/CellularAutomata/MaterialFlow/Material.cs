using System.Runtime.InteropServices;
using CellularAutomata.Cells;

namespace CellularAutomata.MaterialFlow;

[StructLayout(LayoutKind.Sequential, Size = 16)]
public record struct Material(Vector Position, Cell Body);