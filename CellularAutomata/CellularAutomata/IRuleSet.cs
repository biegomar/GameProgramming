namespace CellularAutomata;

public interface IRuleSet<T> : IBaseRuleSet
{
    T ApplyRules(IPlayGround<T> playGround, Vector position);
    T ApplyRules(IPlayGround<T> playGround, (int X, int Y) position);
    IPlayGround<T> ApplySpawnRules(IPlayGround<T> playGround, bool isSpawn);
}