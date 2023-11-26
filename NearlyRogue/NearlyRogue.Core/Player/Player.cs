using NearlyRogue.Core.Dices;
using NearlyRogue.Core.FightSystem;

namespace NearlyRogue.Core.Player;

public class Player : IAttacker, IDefender
{
    public string Name { get; init; }
    public uint Gold { get; set; }
    public ushort Experience { get; set; }
    public sbyte ExperienceLevel { get; set; }
    public DiceThrow HitPoints { get; set; }
    public ushort Strength { get; set; }
    public sbyte AmorClass { get; set; }

    private readonly IAttackStrategy attackStrategy;
    private readonly IDefendStrategy defendStrategy;
    private readonly IExperienceCalculator experienceCalculator;

    public Player(IAttackStrategy attackStrategy, IDefendStrategy defendStrategy, IExperienceCalculator experienceCalculator)
    {
        this.attackStrategy = attackStrategy;
        this.defendStrategy = defendStrategy;
        this.experienceCalculator = experienceCalculator;
    }
    
    public void Attack(IDefender villain)
    {
        this.attackStrategy.Attack(this, villain);
    }

    public void Defeat(IAttacker attacker)
    {
        this.defendStrategy.Defeat(attacker, this);
    }
}