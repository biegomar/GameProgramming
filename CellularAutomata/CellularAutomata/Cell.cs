using System.Runtime.InteropServices;

namespace CellularAutomata;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public record struct Cell(CellState State)
{
    public CellState State = State;
}