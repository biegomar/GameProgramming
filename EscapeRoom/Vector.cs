using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    /// <summary>
    /// 2D-Vektor für die Spielfeld-Dimension, die Playerposition, ...
    /// </summary>
    internal sealed record Vector(int X, int Y);
}
