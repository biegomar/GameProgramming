namespace CellularAutomata;

public interface IRuleSet : IBaseRuleSet
{
    CellState ApplyRules(IPlayGround playGround, Vector position);
    IPlayGround ApplySpawnRules(IPlayGround playGround, Vector spawnPosition);
}