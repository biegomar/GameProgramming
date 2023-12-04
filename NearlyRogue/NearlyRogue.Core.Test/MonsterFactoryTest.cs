using NearlyRogue.Core.Monsters;

namespace NearlyRogue.Core.Test;

public class MonsterFactoryTest
{
    [Theory]
    [InlineData(MonsterRace.Aquator, 5, 2, 20, 0, MonsterFlags.Mean)]
    [InlineData(MonsterRace.Bat, 1, 3, 1,0, MonsterFlags.Flying)]
    [InlineData(MonsterRace.Centaur, 4,4,25, 15, null)]
    [InlineData(MonsterRace.Dragon, 10, -1,6800, 100, MonsterFlags.Mean)]
    [InlineData(MonsterRace.Emu, 1,7,2,0, MonsterFlags.Mean)]
    [InlineData(MonsterRace.VenusFlytrap, 8,3, 80,0, MonsterFlags.Mean)]
    [InlineData(MonsterRace.Griffin,13,2,2000,20, MonsterFlags.Mean | MonsterFlags.Flying | MonsterFlags.Regeneration)]
    [InlineData(MonsterRace.Hobgoblin, 1,5,3,0, MonsterFlags.Mean)]
    [InlineData(MonsterRace.IceMonster, 1,9,15,0, MonsterFlags.Mean)]
    [InlineData(MonsterRace.Jabberwock, 15,6,4000,70, null)]
    [InlineData(MonsterRace.Kestrel, 1,7,1,0, MonsterFlags.Mean | MonsterFlags.Flying)]
    [InlineData(MonsterRace.Leprechaun, 3,8,10, 0, MonsterFlags.Greedy)]
    [InlineData(MonsterRace.Medusa, 8,2,200,40, MonsterFlags.Mean)]
    [InlineData(MonsterRace.Nymph, 3,9, 37, 100, null)]
    [InlineData(MonsterRace.Orc, 1,6, 5,15, MonsterFlags.Greedy)]
    [InlineData(MonsterRace.Phantom, 8,3,120,0, MonsterFlags.Invisible)]
    [InlineData(MonsterRace.Quagga, 3,2,32,30, MonsterFlags.Mean)]
    [InlineData(MonsterRace.Rattlesnake,2,3,9,0, MonsterFlags.Mean)]
    [InlineData(MonsterRace.Snake, 2,8, 1,0, MonsterFlags.Mean)]
    [InlineData(MonsterRace.Troll, 6,4,120,50, MonsterFlags.Mean | MonsterFlags.Regeneration)]
    [InlineData(MonsterRace.Urvile, 7,-2,190,0, MonsterFlags.Mean)]
    [InlineData(MonsterRace.Vampire, 8,1,350,20, MonsterFlags.Mean | MonsterFlags.Regeneration)]
    [InlineData(MonsterRace.Wraith, 5,4,55,0, null)]
    [InlineData(MonsterRace.Xeroc, 7,7,100, 30, null)]
    [InlineData(MonsterRace.Yeti, 4,6,50, 30, null)]
    [InlineData(MonsterRace.Zombie, 2,8,6, 0, MonsterFlags.Mean)]
    public void CreateAllMonsterShouldWork(MonsterRace monsterRace, sbyte level, sbyte amor, ushort experience, byte treasurePercentage, MonsterFlags? flags)
    {
        //arrange
        var sut = new MonsterFactory();
        
        //act
        var actual = sut.CreateMonster(monsterRace);
        
        //assert
        Assert.Equal(monsterRace, actual.Race);
        Assert.Equal(level, actual.ExperienceLevel);
        Assert.Equal(amor, actual.AmorClass);
        Assert.Equal(experience, actual.Experience);
        Assert.Equal(treasurePercentage, actual.TreasurePercentage);
        Assert.Equal(flags, actual.Flags);
    }
}