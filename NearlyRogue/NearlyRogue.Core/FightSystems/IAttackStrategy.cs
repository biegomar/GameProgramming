namespace NearlyRogue.Core.FightSystems;

public interface IAttackStrategy
{
    void Attack(IAttacker attacker, IDefender defender);
}