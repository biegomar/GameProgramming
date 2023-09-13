using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    internal class WinException : Exception
    {
        internal WinException(string message) : base(message)
        {
        }
    }
}
