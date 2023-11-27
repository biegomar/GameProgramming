namespace NearlyRogue.Core.FightSystems;

public class FightSystem
{
    private readonly IBattleStrategy BattleStrategy;
    
    public FightSystem(IBattleStrategy battleStrategy)
    {
        this.BattleStrategy = battleStrategy;
    }
    public void BattleOneRound(IAttacker attacker, IDefender defender)
    {
        this.BattleStrategy.BattleOneRound(attacker, defender);
    }
}