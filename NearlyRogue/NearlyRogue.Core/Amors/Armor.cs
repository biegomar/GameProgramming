namespace NearlyRogue.Core.Amors;

public class Armor
{
    public ArmorType Type { get; set; }
    public ArmorFlags Flags { get; set; }
    public byte Count { get; set; }
    public sbyte AmorClass { get; set; }
}