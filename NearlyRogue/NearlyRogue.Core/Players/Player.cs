using System.Collections.Immutable;
using NearlyRogue.Core.Dices;
using NearlyRogue.Core.FightSystems;
using NearlyRogue.Core.Weapons;

namespace NearlyRogue.Core.Players;

public class Player : ICreature
{
    public required string Name { get; init; }
    public uint Gold { get; set; }
    public ushort Experience { get; set; }
    public byte ExperienceLevel { get; set; }
    public ushort HitPoints { get; set; }
    public ushort MaxHitPoints { get; set; }
    public ushort Strength { get; set; }
    public sbyte AmorClass { get; set; }
    public byte Food { get; set; }
    public Weapon ActiveWeapon { get; set; }
    public IList<Weapon> Weapons { get; set; }
    public required IList<DiceThrow> Damage { get; init; }
    
    private readonly IExperienceCalculator experienceCalculator;

    public Player(IExperienceCalculator experienceCalculator)
    {
        this.experienceCalculator = experienceCalculator;
        this.Weapons = new List<Weapon>();
    }
}