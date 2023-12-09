namespace NearlyRogue.Core.FightSystems;

public interface ICreature
{
    public sbyte ExperienceLevel { get; set; }
    public ushort Strength { get; set; }
    public sbyte AmorClass { get; set; }
}