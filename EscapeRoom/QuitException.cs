﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    internal class QuitException : Exception
    {
        internal QuitException(string message) : base(message)
        {
        }
    }
}
