using NearlyRogue.Core.Dices;

namespace NearlyRogue.Core.Test;

public class DiceTest
{
    private static Random random = new Random(1);
    
    [Theory]
    [InlineData(DiceType.D6)]
    public void DiceRollShouldWork(DiceType diceType)
    {
        //arrange
        var sut = new Dice(diceType, random);
        
        //act
        var actualTry1 = sut.Roll(1);
        var actualTry2 = sut.Roll(1);
        var actualTry3 = sut.Roll(1);
        var actualTry4 = sut.Roll(1);
        var actualTry5 = sut.Roll(1);
        var actualTry6 = sut.Roll(1);
        var actualTry7 = sut.Roll(1);
        var actualTry8 = sut.Roll(1);
        
        //assert (seeding determines random, so we always expect the same results)
        Assert.Equal(2, actualTry1);
        Assert.Equal(1, actualTry2);
        Assert.Equal(3, actualTry3);
        Assert.Equal(5, actualTry4);
        Assert.Equal(4, actualTry5);
        Assert.Equal(3, actualTry6);
        Assert.Equal(3, actualTry7);
        Assert.Equal(6, actualTry8);
    }
}