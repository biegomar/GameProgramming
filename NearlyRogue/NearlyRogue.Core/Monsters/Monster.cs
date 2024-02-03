using NearlyRogue.Core.Dices;
using NearlyRogue.Core.FightSystems;

namespace NearlyRogue.Core.Monsters;

public class Monster : ICreature
{
    public MonsterRace Race { get; init; }
    public string Name => Race.ToString();
    public byte TreasurePercentage { get; init; }
    public MonsterFlags Flags { get; init; }
    public ushort Experience { get; set; }
    public byte ExperienceLevel { get; set; }
    public ushort HitPoints { get; set; }
    public ushort MaxHitPoints { get; set; }
    public ushort Strength { get; set; }
    public sbyte AmorClass { get; set; }
    public IList<DiceThrow> Damage { get; init; }
    
    public ushort Range { get; set; }
}