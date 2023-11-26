namespace NearlyRogue.Core.FightSystem;

public interface IBattleStrategy
{
    void BattleOneRound(IAttacker attacker, IDefender defender);
}