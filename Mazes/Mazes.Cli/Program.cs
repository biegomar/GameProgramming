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
            var maze3 = new Maze(new EmptyMazeGenerator(), 10, 10);
            var maze4 = new Maze(new FullMazeGenerator(), 10, 10);
            
            Console.Clear();
            maze.PrintMazeAtColumn("Binary Tree", 0);
            maze2.PrintMazeAtColumn("Sidewinder", 45);
            maze3.PrintMazeAtColumn("Empty", 90);
            maze4.PrintMazeAtColumn("Full", 135);
        }
    }
}