using Mazes.BinaryTreeMaze;
using Mazes.Contracts;

namespace Mazes.Cli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var maze = new Maze(new BinaryTreeMazeGenerator(), 10, 10);

            Console.WriteLine(maze);
        }
    }
}