namespace NearlyRogue.Core.FightSystems;

public interface IDefendStrategy
{
    void Defeat(IAttacker attacker, IDefender defender);
}