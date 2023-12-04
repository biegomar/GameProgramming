namespace NearlyRogue.Core.FightSystems;

public interface IDefender : ICreature
{
    void Defeat(IAttacker attacker);
}