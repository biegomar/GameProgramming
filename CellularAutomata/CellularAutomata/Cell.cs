namespace CellularAutomata;

public sealed class Cell<T>(T state, int x, int y)
{
    public readonly int X = x;
    public readonly int Y = y;
    public T State = state;
}