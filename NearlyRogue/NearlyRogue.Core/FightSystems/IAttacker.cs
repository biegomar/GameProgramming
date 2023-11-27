namespace NearlyRogue.Core.FightSystems;

public interface IAttacker
{
    void Attack(IDefender villain);
}