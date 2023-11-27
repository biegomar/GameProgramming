namespace NearlyRogue.Core.FightSystems.StandardImpl;

public class StandardBattleStrategy : IBattleStrategy
{
    public void BattleOneRound(IAttacker attacker, IDefender defender)
    {
        attacker.Attack(defender);
    }
}