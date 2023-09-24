using MonsterAttack;

namespace TestMonsterAttack
{
    public class StandardAttackStrategyTest
    {
        [Fact]
        public void AttackTest()
        {
            //Arrange
            var sut = new StandardAttackStrategy();
            var monster1 = new Monster(sut)
            {
                R = MonsterClass.Ork,
                HP = 120,
                AP = 10,
                DP = 10,
                S = 10
            };

            var monster2 = new Monster(sut)
            {
                R = MonsterClass.Troll,
                HP = 120,
                AP = 10,
                DP = 8,
                S = 10
            };

            var expected = 2.0f;

            //Act
            var actual = sut.Attack(monster1, monster2);

            //Assert
            Assert.Equal(expected, actual);

        }
    }
}