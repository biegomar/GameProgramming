using BinaryTreeMaze;
using Mazes.Contracts;
using SideWinderMaze;

namespace Mazes.Cli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dimension = new MazeVector(10, 10, 0);
            var maze = new Maze<char>(dimension, new BinaryTreeMazeGenerator(), new ConsoleMazePrinter(), "Binary Tree");
            var maze2 = new Maze<char>(dimension, new SideWinderMazeGenerator(), new ConsoleMazePrinter(), "Sidewinder");
            var maze3 = new Maze<char>(dimension, new EmptyMazeGenerator(), new ConsoleMazePrinter(), "Empty");
            var maze4 = new Maze<char>(dimension, new FullMazeGenerator(), new ConsoleMazePrinter(), "Full");
            
            maze.SetCellItem(new CellItem<char>('X', new MazeVector(1,1,0)));
            maze2.SetCellItem(new CellItem<char>('B', new MazeVector(0,0,0)));
            maze3.SetCellItem(new CellItem<char>('O', new MazeVector(9,9,0)));
            maze4.SetCellItem(new CellItem<char>('I', new MazeVector(4,5,0)));
            
            
            Console.Clear();
            
            maze.Draw(new MazeVector(0,0,0));
            maze2.Draw(new MazeVector(45,0,0));
            maze3.Draw(new MazeVector(90,0,0));
            maze4.Draw(new MazeVector(135,0,0));
            
            maze.DrawCellItems();
            maze2.DrawCellItems();
            maze3.DrawCellItems();
            maze4.DrawCellItems();
        }
    }
}