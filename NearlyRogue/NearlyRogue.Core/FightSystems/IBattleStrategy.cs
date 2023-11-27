namespace NearlyRogue.Core.FightSystems;

public interface IBattleStrategy
{
    void BattleOneRound(IAttacker attacker, IDefender defender);
}