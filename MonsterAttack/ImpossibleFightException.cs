using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterAttack
{
    internal class ImpossibleFightException : Exception
    {
        internal ImpossibleFightException(string message) : base(message)
        {
        }
    }
}
