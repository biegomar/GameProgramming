namespace Mazes.Contracts
{
    public class MazeVector
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public MazeVector(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override string ToString()
        {
            return $"[{this.X}, {this.Y}, {this.Z}]";
        }
    }
}