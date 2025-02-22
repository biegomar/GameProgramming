using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;

namespace CellularAutomata;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public sealed class Cell<T>(T state, int x, int y)
{
    public int X = x;
    public int Y = y;
    public T State = state;
}