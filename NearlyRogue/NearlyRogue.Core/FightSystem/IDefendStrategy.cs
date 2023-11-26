namespace NearlyRogue.Core.FightSystem;

public interface IDefendStrategy
{
    void Defeat(IAttacker attacker, IDefender defender);
}