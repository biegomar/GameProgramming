using BinareTreeMaze;
using Mazes.Contracts;

namespace Mazes.Cli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var maze = new Maze(new BinareTreeMazeGenerator(), 10, 10);
        }
    }
}