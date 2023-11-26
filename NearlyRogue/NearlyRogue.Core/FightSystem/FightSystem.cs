namespace NearlyRogue.Core.FightSystem;

public class FightSystem
{
    private readonly IBattleStrategy BattleStrategy;
    
    public FightSystem(IBattleStrategy battleStrategy)
    {
        this.BattleStrategy = battleStrategy;
    }
    void BattleOneRound(IAttacker attacker, IDefender defender)
    {
        this.BattleStrategy.BattleOneRound(attacker, defender);
    }
}