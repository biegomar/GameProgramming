namespace CellularAutomata;

public interface IRuleSet<T> : IBaseRuleSet
{
    T ApplyRules(PlayGround<T> playGround, Vector position);
    T ApplyRules(PlayGroundArray<T> playGround, (int X, int Y, int Z) position);
    PlayGround<T> ApplySpawnRules(PlayGround<T> playGround, bool isSpawn);
    PlayGroundArray<T> ApplySpawnRules(PlayGroundArray<T> playGround, bool isSpawn);
}