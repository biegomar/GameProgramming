namespace NearlyRogue.Core.FightSystems;

public interface ICreature
{
    public ushort Experience { get; set; }
    public sbyte ExperienceLevel { get; set; }
    public ushort Strength { get; set; }
    public sbyte AmorClass { get; set; }
}