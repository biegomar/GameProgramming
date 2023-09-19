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
            Console.WriteLine($"Der {this.R} attackiert den {villain.R}!");
            
            float damage = this.attackStrategy.Attack(this, villain);            
            Console.WriteLine($"Der Angriff des {this.R} verursacht {damage} Schaden!");

            villain.HP = Math.Max(0, villain.HP - damage);            

            if (villain.HP == 0)
            {
                throw new KillException($"Der {villain.R} ist tot!");
            }

            Console.WriteLine($"Uhh! Das war heftig! Der {villain.R} hat nur noch {villain.HP} Lebenspunkte!");

            Console.WriteLine();
            Console.WriteLine("------ Nächste Runde -------");
        }
    }
}
