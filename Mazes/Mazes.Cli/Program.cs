using AldousBroderMaze;
using BinaryTreeMaze;
using MazePathFinder;
using Mazes.Contracts;
using Mazes.Contracts.PathFinding;
using SideWinderMaze;

namespace Mazes.Cli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            
            var dimension = new MazeVector(10, 10, 0);
            var maze = new Maze<char>(dimension, new BinaryTreeMazeGenerator<char>(), new ConsoleMazePrinter<char>(), "Binary Tree");
            var maze2 = new Maze<char>(dimension, new SideWinderMazeGenerator<char>(), new ConsoleMazePrinter<char>(), "Sidewinder");
            var maze3 = new Maze<char>(dimension, new EmptyMazeGenerator<char>(), new ConsoleMazePrinter<char>(), "Empty");
            var maze4 = new Maze<char>(dimension, new FullMazeGenerator<char>(), new ConsoleMazePrinter<char>(), "Full");

            
            var numberOfAgents = 5;
            var listOfItems = new List<char>();
            for (int i = 0; i < numberOfAgents; i++)
            {
                listOfItems.Add(Convert.ToChar(i.ToString()));
            }

            // var maze5 = new Maze<char>(dimension,
            //     new AldousBroderMazeGenerator<char>(new ConsoleMazePrinter<char>(listOfItems), numberOfAgents),
            //     new ConsoleMazePrinter<char>(), "AldousBroder");

            var maze5 = new Maze<char>(dimension, new AldousBroderMazeGenerator<char>(null),
                new ConsoleMazePrinter<char>(), "AldousBroder");
            
            
            maze.SetCellItem(new CellItem<char>('A', new MazeVector(1,1,0)));
            maze2.SetCellItem(new CellItem<char>('B', new MazeVector(0,0,0)));
            maze3.SetCellItem(new CellItem<char>('O', new MazeVector(9,9,0)));
            maze4.SetCellItem(new CellItem<char>('I', new MazeVector(4,5,0)));
            maze5.SetCellItem(new CellItem<char>('X', new MazeVector(2,4,0)));
            
            Console.Clear();
            
            //maze5.Draw(new MazeVector(0,0,0));
            
            //maze5.DrawItemAtPosition(new MazeVector(0,0,0), 'X');
            
            //maze.Draw(new MazeVector(0,0,0));
            //maze2.Draw(new MazeVector(45,0,0));
            //maze3.Draw(new MazeVector(90,0,0));
            //maze4.Draw(new MazeVector(135,0,0));
            maze5.Draw(new MazeVector(0,0,0));
            
            //maze.DrawCellItems();
            //maze2.DrawCellItems();
            //maze3.DrawCellItems();
            //maze4.DrawCellItems();
            

            var pathFinder = new PathFinderForMaze<char>(maze5);
            var path = pathFinder.GetShortestPath(new MazeVector(2, 3, 0), new MazeVector(8, 9, 0));

            foreach (var vector in path)
            {
                maze5.SetCellItem(new CellItem<char>(vector.Z.ToString("X1").ToCharArray()[0], vector));
            }
            maze5.DrawCellItems();
            
            Console.Write(string.Join(", ", path));

            Console.ReadKey();
        }
    }
}