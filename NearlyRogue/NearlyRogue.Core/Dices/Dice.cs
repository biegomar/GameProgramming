namespace NearlyRogue.Core.Dices;

public class Dice
{
    private readonly Random random = new();
    
    public DiceType TypeOfDice { get; init; }
    public byte Sides => (byte)this.TypeOfDice;

    public ushort Roll(sbyte tries)
    {
        ushort result = 0;

        for (sbyte i = 0; i < tries; i++)
        {
            result += (ushort)random.Next(this.Sides);
        }

        return result;
    }
}