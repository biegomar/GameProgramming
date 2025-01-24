namespace CellularAutomata;

public interface IRuleSet<T>
{
    public PlayGround<T> PlayGround { get; init; }
    
    public T ApplyRules(Vector position);
}