namespace NearlyRogue.Core.FightSystems;

public interface IAttacker : ICreature
{
    void Attack(IDefender villain);
}