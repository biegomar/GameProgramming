using NearlyRogue.Core.Dices;
using NearlyRogue.Core.FightSystems;
using NearlyRogue.Core.Weapons;

namespace NearlyRogue.Core.Players;

public class Player : IAttacker, IDefender
{
    public required string Name { get; init; }
    public uint Gold { get; set; }
    public ushort Experience { get; set; }
    public sbyte ExperienceLevel { get; set; }
    public ushort HitPoints { get; set; }
    public ushort MaxHitPoints { get; set; }
    public ushort Strength { get; set; }
    public sbyte AmorClass { get; set; }
    public WeaponType ActiveWeapon { get; set; }
    public required IList<DiceThrow> Damage { get; set; }
    public byte Food { get; set; }

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