using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    /// <summary>
    /// 2D-Koordinate für die Spielfeld-Dimension, die Playerposition, ...
    /// </summary>
    internal sealed record Coordinate(int X, int Y);
}
