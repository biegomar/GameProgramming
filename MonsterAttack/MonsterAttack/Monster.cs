using System.Text;

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
        /// MonsterClass (R).
        /// </summary>
        internal MonsterClass R { get; set; }

        /// <summary>
        /// The visual representation of the monster.
        /// </summary>
        public string Avatar { get; set; } = string.Empty;

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

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine(this.Avatar);
            result.AppendLine();
            result.AppendLine($"Attack: {this.AP} - Defense: {this.DP} - Speed: {this.S} - Actual Health: {this.HP}");

            return result.ToString();
        }
    }
}
