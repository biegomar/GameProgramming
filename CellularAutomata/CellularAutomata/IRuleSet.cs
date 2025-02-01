namespace CellularAutomata;

public interface IRuleSet<T>
{
    T ApplyRules(PlayGround<T> playGround, Vector position);
    PlayGround<T> ApplySpawnRules(PlayGround<T> playGround);
}