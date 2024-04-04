namespace Mazes.Contracts
{
    public interface IMazePrinter
    {
        void DrawMaze<T>(Maze<T> maze, MazeVector startMazeVector);
        void DrawCellItems<T>(Maze<T> maze);
    }
}