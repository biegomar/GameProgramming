using System.Runtime.InteropServices;

namespace CellularAutomata;

    /// <summary>
    /// The vector class.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector
    {
        /// <summary>
        /// X
        /// </summary>
        public int X;

        /// <summary>
        /// Y
        /// </summary>
        public int Y;

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="x">value x.</param>
        /// <param name="y">value y.</param>
        /// <param name="z">value z.</param>
        public Vector(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        

        /// <summary>
        /// Operator overload == (equality).
        /// </summary>
        /// <param name="v1">The first vector to compare.</param>
        /// <param name="v2">The second vector to compare.</param>
        /// <returns>The resulting vector.</returns>
        public static bool operator ==(Vector v1, Vector v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        /// <summary>
        /// Operator overload != (inequality).
        /// </summary>
        /// <param name="v1">The first vector to compare.</param>
        /// <param name="v2">The second vector to compare.</param>
        /// <returns>The resulting vector.</returns>
        public static bool operator !=(Vector v1, Vector v2)
        {
            return !(v1 == v2);
        }       

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Vector other)
            {
                return this == other;
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        
        
        public override string ToString()
        {
            return $"[{this.X}, {this.Y}]";
        }
    }