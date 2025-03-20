namespace CellularAutomata;

public interface IRuleSet : IBaseRuleSet
{
    CellState ApplyRules(IPlayGround playGround, Vector position);
    CellState ApplyRules(IPlayGround playGround, (int X, int Y) position);
    IPlayGround ApplySpawnRules(IPlayGround playGround, bool isSpawn);
}