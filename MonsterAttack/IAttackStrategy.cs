using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="villain">The villain to attack</param>
        /// <returns>damage done.</returns>
        float Attack(Monster attacker, Monster villain);
    }
}
