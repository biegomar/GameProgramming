using System.Runtime.InteropServices;

namespace CellularAutomata;

[StructLayout(LayoutKind.Sequential)]
public record struct Cell<T>(T State)
{
    public T State = State;
}