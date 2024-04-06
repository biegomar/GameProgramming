using System.Runtime.CompilerServices;

namespace Mazes.Contracts
{
    public interface IMazePrinter
    {
        public void DrawMaze<T>(Maze<T> maze, MazeVector startMazeVector, bool drawItems = false)
        {
            this.DrawMaze(maze.Cells, startMazeVector, maze.Title, drawItems);    
        }
        
        public void DrawMaze<T>(Cell<T>[,] cells, MazeVector startMazeVector, string title, bool drawItems = false);

        public void DrawCellItems<T>(Maze<T> maze)
        {
            this.DrawCellItems(maze.Cells);    
        }
        
        public void DrawCellItems<T>(Cell<T>[,] cells);
    }
}