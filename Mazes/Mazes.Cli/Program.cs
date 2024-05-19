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
        private static IDictionary<int, char> pathSigns = new Dictionary<int, char>();

        static void Main(string[] args)
        {
            FillPathSigns();
            
            Console.Clear();
            
            var dimension = new CellVector(10, 10, 0);

            Landscape<char> maze;

            // maze = GenerateMaze(new BinaryTreeMazeGenerator<char>(), dimension, CellVector.Zero,
            //     new CellItem<char>('A', new CellVector(1, 1, 0)), "BinaryTree");
            
            // maze = GenerateMaze(new SideWinderMazeGenerator<char>(), dimension, CellVector.Zero,
            //     new CellItem<char>('B', CellVector.Zero), "Sidewinder");
            
            // maze = GenerateMaze(new EmptyMazeGenerator<char>(), dimension, CellVector.Zero,
            //     new CellItem<char>('O', new CellVector(9,9,0)), "Empty");
            
            // maze = GenerateMaze(new FullMazeGenerator<char>(), dimension, CellVector.Zero,
            //     new CellItem<char>('I', new CellVector(4,5,0)), "Full");
            
            maze = GenerateMaze(new AldousBroderMazeGenerator<char>(null), dimension, CellVector.Zero,
                new CellItem<char>('X', new CellVector(2,4,0)), "AldousBroder");
            
            FindPath(maze);

            Console.ReadKey();
        }

        private static void FillPathSigns()
        {
            int key = 0;
            for (int value = 48; value <= 57; value++)
            {
                pathSigns.Add(key, (char)value);
                key++;
            }
            for (int value = 65; value <= 90; value++)
            {
                pathSigns.Add(key, (char)value);
                key++;
            }
            for (int value = 97; value <= 122; value++)
            {
                pathSigns.Add(key, (char)value);
                key++;
            }
        }

        private static void FindPath(Landscape<char> maze)
        {
            var pathFinder = new PathFinderForMaze<char>(maze);
            var path = pathFinder.GetShortestPath(new CellVector(2, 3, 0), new CellVector(8, 9, 0));

            foreach (var vector in path)
            {
                maze.SetCellItem(new CellItem<char>(pathSigns[vector.Z], vector));
            }
            maze.DrawCellItems();
            
            Console.Write(string.Join(", ", path));
        }

        private static Landscape<char> GenerateMaze(IProceduralContentGenerator<char> generator, CellVector dimension, CellVector screenPosition, CellItem<char> item, string title)
        {
            var maze = new Landscape<char>(dimension, generator, new ConsoleMazePrinter<char>(), title);
            maze.SetCellItem(item);
            Console.Clear();
            maze.Draw(screenPosition);
            maze.DrawCellItems();

            return maze;
        }
    }
}