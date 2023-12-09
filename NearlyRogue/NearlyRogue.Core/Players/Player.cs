using NearlyRogue.Core.Dices;
using NearlyRogue.Core.FightSystems;
using NearlyRogue.Core.Weapons;

namespace NearlyRogue.Core.Players;

public class Player : ICreature
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
    
    private readonly IExperienceCalculator experienceCalculator;

    public Player(IExperienceCalculator experienceCalculator)
    {
        this.experienceCalculator = experienceCalculator;
    }
}