using BinareTreeMaze;
using Mazes.Contracts;
using SideWinderMaze;

namespace Mazes.Cli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var maze = new Maze(new BinareTreeMazeGenerator(), 10, 10);
            var maze2 = new Maze(new SideWinderMazeGenerator(), 10, 10);
            
            Console.WriteLine("Binary Tree");
            Console.WriteLine(maze);
            Console.WriteLine("Sidewinder");
            Console.WriteLine(maze2);
        }
    }
}