using MonsterAttack;

namespace TestMonsterAttack
{
    
    public class MonsterTest
    {
        [Fact]
        public void AttackAndKillVillainTest()
        {
            // Arrange
            var testStrategy = new TestAttackStrategy();

            var sut = new Monster(testStrategy)
            {
                R = MonsterClass.Ork,
                HP = 120,
                AP = 10,
                DP = 10,
                S = 10
            };

            var villain = new Monster(testStrategy)
            {
                R = MonsterClass.Troll,
                HP = 120,
                AP = 10,
                DP = 8,
                S = 10
            };

            // Act & Assert
            Assert.Throws<KillException>(() => sut.Attack(villain));
        }
    }
}
