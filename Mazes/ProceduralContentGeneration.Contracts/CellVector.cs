namespace Mazes.Contracts
{
    public class CellVector
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public CellVector(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override string ToString()
        {
            return $"[{this.X}, {this.Y}, {this.Z}]";
        }

        public static CellVector Zero => new CellVector(0, 0, 0);
    }
}