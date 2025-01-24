namespace CellularAutomata;

public interface IRuleSet<T>
{
    public T ApplyRules(PlayGround<T> playGround, Vector position);
}