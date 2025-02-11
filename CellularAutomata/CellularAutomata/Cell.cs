namespace CellularAutomata;

public record Cell<T>
{
    public T State { get; set; }

    public (int X, int Y, int Z) Position { get; init; }
    
    public Cell(T state, (int X, int Y, int Z) position)
    {
        State = state;
        Position = position;
    }
}