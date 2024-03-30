using BinareTreeMaze;
using Mazes.Contracts;
using SideWinderMaze;

namespace Mazes.Cli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dimension = new MazeVector(10, 10, 0);
            var maze = new Maze(new BinareTreeMazeGenerator(), dimension, "Binary Tree");
            var maze2 = new Maze(new SideWinderMazeGenerator(), dimension, "Sidewinder");
            var maze3 = new Maze(new EmptyMazeGenerator(), dimension, "Empty");
            var maze4 = new Maze(new FullMazeGenerator(), dimension, "Full");
            
            maze3.SetCellItem(new CellItem('I', 4,4));
            
            Console.Clear();
            maze.DrawMaze(new MazeVector());
            maze2.DrawMaze(new MazeVector(45));
            maze3.DrawMaze(new MazeVector(90));
            maze4.DrawMaze(new MazeVector(135));

            Console.ReadKey();
            
            maze3.RedrawMaze();
        }
    }
}