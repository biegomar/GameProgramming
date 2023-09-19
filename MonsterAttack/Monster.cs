using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterAttack
{
    /// <summary>
    /// The monster.
    /// </summary>
    internal sealed class Monster
    {
        private readonly IAttackStrategy attackStrategy;

        internal Monster(IAttackStrategy attackStrategy)
        {
            this.attackStrategy = attackStrategy;
        }

        /// <summary>
        /// Health points (HP).
        /// </summary>
        internal float HP { get; set; }

        /// <summary>
        /// Attack points (AP).
        /// </summary>
        internal float AP { get; set; }

        /// <summary>
        /// Defense points (DP).
        /// </summary>
        internal float DP { get; set; }

        /// <summary>
        /// Speed (S).
        /// </summary>
        internal float S { get; set; }

        /// <summary>
        /// Race (R).
        /// </summary>
        internal Race R { get; set; }     
        
        /// <summary>
        /// Attack!
        /// </summary>
        /// <param name="villain"></param>
        internal void Attack(Monster villain)
        {
            float damage = this.attackStrategy.Attack(this, villain);

            villain.HP = Math.Max(0, villain.HP - damage);

            if (villain.HP == 0)
            {
                throw new KillException($"The {villain.R} is dead!");
            }
        }
    }
}
