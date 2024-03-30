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
            
            maze3.SetCellItem(new CellItem('I', 4,4));
            
            Console.Clear();
            maze.DrawMaze("Binary Tree", 0);
            maze2.DrawMaze("Sidewinder", 45);
            maze3.DrawMaze("Empty", 90);
            maze4.DrawMaze("Full", 135);

            Console.ReadKey();
            
            maze3.RedrawMaze();
        }
    }
}