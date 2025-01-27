namespace CellularAutomata;

public record Cell<T>
{
    public T State { get; set; }
    
    public Cell(T state)
    {
        State = state;
    }
}