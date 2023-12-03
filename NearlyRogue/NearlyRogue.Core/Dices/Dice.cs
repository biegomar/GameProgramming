namespace NearlyRogue.Core.Dices;

public class Dice
{
    private readonly Random random;
    
    public DiceType TypeOfDice { get; init; }
    public byte Sides => (byte)this.TypeOfDice;

    public Dice(DiceType typeOfDice)
    {
        this.random = new();
        this.TypeOfDice = typeOfDice;
    }
    
    public Dice(DiceType typeOfDice, Random random)
    {
        this.random = random;
        this.TypeOfDice = typeOfDice;
    }
    
    public ushort Roll(sbyte tries)
    {
        ushort result = 0;

        for (sbyte i = 0; i < tries; i++)
        {
            result += (ushort)random.Next(1, this.Sides + 1);
        }

        return result;
    }
}