using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Mazes.Contracts
{
    public interface IContentPrinter<T>
    {
        public IList<T>? Items { get; set; }
        
        public void DrawMaze(Maze<T> maze, MazeVector startMazeVector, bool drawItems = false)
        {
            this.DrawMaze(maze.Cells, startMazeVector, maze.Title, drawItems);    
        }
        
        public void DrawMaze(Cell<T>?[,] cells, MazeVector startMazeVector, string title, bool drawItems = false);

        public void DrawCellItems(Maze<T> maze)
        {
            this.DrawCellItems(maze.Cells);    
        }
        
        public void DrawCellItems(Cell<T>?[,] cells);

        public void DrawItemAtPosition(Maze<T> maze, MazeVector position, T item)
        {
            this.DrawItemAtPosition(maze.Cells, position, item);
        }
        
        public void DrawItemAtPosition(Cell<T>?[,] cells, MazeVector position, T item);
    }
}