namespace MonsterAttack
{
    /// <summary>
    /// The interface for the attack strategy.
    /// </summary>
    internal interface IAttackStrategy
    {
        /// <summary>
        /// The real attack!
        /// </summary>
        /// <param name="attacker">The attacker.</param>
        /// <param name="villain">The villain to attack.</param>
        /// <returns>damage done.</returns>
        float Attack(Monster attacker, Monster villain);

        /// <summary>
        /// Check whether a fight is possible under the circumstances.
        /// </summary>
        /// <param name="attacker">The attacker.</param>
        /// <param name="villain">The villain to attack.</param>
        /// <returns>true if fight is possible. Otherwise false.</returns>
        bool IsFightPossible(Monster attacker, Monster villain);
    }
}
