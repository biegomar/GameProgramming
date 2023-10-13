namespace Vectors
{
    /// <summary>
    /// The vector class.
    /// The calculations are based on general vector mathematics.
    /// </summary>
    public class Vector
    {
        /// <summary>
        /// X
        /// </summary>
        public float X { get; init; }
        
        /// <summary>
        /// Y
        /// </summary>
        public float Y { get; init; }
        
        /// <summary>
        /// Z
        /// </summary>
        public float Z { get; init; }

        /// <summary>
        /// The base constructor.
        /// </summary>
        public Vector() : this(0, 0, 0) { }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="x">value x.</param>
        /// <param name="y">value y.</param>
        /// <param name="z">value z.</param>
        public Vector(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Operator overload +.
        /// </summary>
        /// <param name="v1">The first vector to add.</param>
        /// <param name="v2">The second vectore to add.</param>
        /// <returns>The resulting vector.</returns>
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        /// <summary>
        /// Operator overload -.
        /// </summary>
        /// <param name="v1">The first vector to sub.</param>
        /// <param name="v2">The second vectore to sub.</param>
        /// <returns>The resulting vector.</returns>
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        /// <summary>
        /// Operator overload * (scalar multiplication).
        /// </summary>
        /// <param name="v1">The vector to scale.</param>
        /// <param name="scalar">The scale factor.</param>
        /// <returns>The resulting vector.</returns>
        public static Vector operator *(Vector v1, float scalar) 
        { 
            return new Vector(v1.X * scalar, v1.Y * scalar, v1.Z * scalar);
        }

        /// <summary>
        /// The length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        public float Length()
        {
            return (float)Math.Sqrt(SquaredLength());
        }

        /// <summary>
        /// The squared length of the vector.
        /// </summary>
        /// <returns>The squared length of the vector.</returns>
        public float SquaredLength()
        {
            return PowerOfTwo(this.X) + PowerOfTwo(this.Y) + PowerOfTwo(this.Z);
        }

        /// <summary>
        /// The distance to another vector.
        /// </summary>
        /// <param name="v">The vector to which the distance is to be calculated.</param>
        /// <returns>The distance of the two vectors.</returns>
        public float Distance(Vector v)
        {
            return Distance(this, v);
        }

        /// <summary>
        /// The distance of two vectors.
        /// </summary>
        /// <param name="v1">The first vector to which the distance is to be calculated.</param>
        /// <param name="v2">The second vector to which the distance is to be calculated.</param>
        /// <returns>The distance of the two vectors.</returns>
        public static float Distance(Vector v1, Vector v2)
        {
            var differenceVector = v1 - v2;
            var lengthOfDifferenceVector = differenceVector.Length();

            return lengthOfDifferenceVector;
        }

        private float PowerOfTwo(float value)
        {
            return value * value;
        }

    }
}