using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterAttack;

namespace TestMonsterAttack
{
    internal class TestAttackStrategy : IAttackStrategy
    {
        public float Attack(Monster attacker, Monster villain)
        {
            // kills always!
            return villain.HP;
        }

        public bool isFightPossible(Monster attacker, Monster villain)
        {
            return true;
        }
    }
}
