namespace CellularAutomata;

public class Cell<T>
{
    public T State { get; set; }
    
    public Cell(T state)
    {
        State = state;
    }
}