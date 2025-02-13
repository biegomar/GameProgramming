using System.Runtime.InteropServices;

namespace CellularAutomata;

[StructLayout(LayoutKind.Sequential)]
public record Cell<T>(T State, (int X, int Y, int Z) Position)
{
    public (int X, int Y, int Z) Position = Position;
    public T State = State;
}