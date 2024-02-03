using NearlyRogue.Core.Dices;

namespace NearlyRogue.Core.FightSystems;

public interface ICreature
{
    public ushort Experience { get; set; }
    public byte ExperienceLevel { get; set; }
    public ushort HitPoints { get; set; }
    public ushort MaxHitPoints { get; set; }
    public ushort Strength { get; set; }
    public sbyte AmorClass { get; set; }
    public IList<DiceThrow> Damage { get; init; }
}