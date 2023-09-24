using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterAttack
{
    internal class KillException : Exception
    {
        internal KillException(string message) : base(message)
        {
        }
    }
}
