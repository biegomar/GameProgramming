namespace NearlyRogue.Core.Numerics;

public struct Vector
{
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public Vector(int x, int y, int z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
}