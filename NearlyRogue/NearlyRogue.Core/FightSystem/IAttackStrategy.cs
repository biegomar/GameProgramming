namespace NearlyRogue.Core.FightSystem;

public interface IAttackStrategy
{
    void Attack(IAttacker attacker, IDefender defender);
}