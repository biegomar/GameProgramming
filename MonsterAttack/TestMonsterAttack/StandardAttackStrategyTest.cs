using MonsterAttack;

namespace TestMonsterAttack
{
    /// <summary>
    /// Test the standard attack strategy.
    /// </summary>
    public class StandardAttackStrategyTest
    {
        /// <summary>
        /// Test the attack.
        /// </summary>
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

        /// <summary>
        /// The if the fight is possible. -> true
        /// </summary>
        [Fact]
        public void IsFightPossibleTrueTest()
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

            var expected = true;

            //Act
            var actual = sut.IsFightPossible(monster1, monster2);

            //Assert
            Assert.Equal(expected, actual);

        }

        /// <summary>
        /// The if the fight is possible. -> false
        /// </summary>
        [Fact]
        public void IsFightPossibleFalseTest()
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
                DP = 10,
                S = 10
            };

            var expected = false;

            //Act
            var actual = sut.IsFightPossible(monster1, monster2);

            //Assert
            Assert.Equal(expected, actual);

        }
    }
}