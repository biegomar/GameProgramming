namespace MonsterAttack
{
    /// <summary>
    /// The standard attack strategy.
    /// </summary>
    internal class StandardAttackStrategy : IAttackStrategy
    {
        public float Attack(Monster attacker, Monster villain)
        {            
            return Math.Max(0, attacker.AP - villain.DP);
        }

        public bool isFightPossible(Monster attacker, Monster villain)
        {
            return attacker.AP > villain.DP || villain.AP > attacker.DP;
        }
    }
}
