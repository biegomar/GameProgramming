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
            var maze = new Maze(dimension, new BinareTreeMazeGenerator(), new ConsoleMazePrinter(), "Binary Tree");
            var maze2 = new Maze(dimension, new SideWinderMazeGenerator(), new ConsoleMazePrinter(), "Sidewinder");
            var maze3 = new Maze(dimension, new EmptyMazeGenerator(), new ConsoleMazePrinter(), "Empty");
            var maze4 = new Maze(dimension, new FullMazeGenerator(), new ConsoleMazePrinter(), "Full");
            
            maze.SetCellItem(new CellItem('X', new MazeVector(1,1,0)));
            maze2.SetCellItem(new CellItem('B', new MazeVector(0)));
            maze3.SetCellItem(new CellItem('O', new MazeVector(9,9,0)));
            maze4.SetCellItem(new CellItem('I', new MazeVector(4,5,0)));
            
            
            Console.Clear();
            
            maze.Draw(new MazeVector());
            maze2.Draw(new MazeVector(45));
            maze3.Draw(new MazeVector(90));
            maze4.Draw(new MazeVector(135));
            
            maze.DrawCellItems();
            maze2.DrawCellItems();
            maze3.DrawCellItems();
            maze4.DrawCellItems();
        }
    }
}