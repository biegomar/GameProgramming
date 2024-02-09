using System.Text;

namespace MonsterAttack
{
    /// <summary>
    /// The monster.
    /// </summary>
    internal sealed class Monster
    {
        private readonly IAttackStrategy attackStrategy;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="attackStrategy">The attack strategy to use in this game.</param>
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
        /// MonsterClass (R).
        /// </summary>
        internal MonsterClass R { get; set; }

        /// <summary>
        /// The visual representation of the monster.
        /// </summary>
        public string[] Avatar { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Attack!
        /// </summary>
        /// <param name="villain"></param>
        internal void Attack(Monster villain)
        {
            Console.WriteLine();
            Console.WriteLine(Utils.WhoAttackedWhom, this.R, villain.R);
            
            var damage = this.attackStrategy.Attack(this, villain);            
            Console.WriteLine(Utils.DamageDone, this.R, damage);

            villain.HP = Math.Max(0, villain.HP - damage);            

            if (villain.HP == 0)
            {
                throw new KillException(string.Format(Utils.VillainIsDead, villain.R));
            }

            if (damage != 0)
            {
                Console.WriteLine(Utils.UuhThatWasHard, villain.R, villain.HP);
            }
            else
            {
                Console.WriteLine(Utils.HitNothing);
            }

            Console.WriteLine();
            Console.WriteLine(Utils.NextRound);
        }        
    }
}
