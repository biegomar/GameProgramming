using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
