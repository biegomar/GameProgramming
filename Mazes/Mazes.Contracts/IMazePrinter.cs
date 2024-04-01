namespace Mazes.Contracts
{
    public interface IMazePrinter
    {
        void DrawMaze(Maze maze, MazeVector startMazeVector);
        void DrawCellItems(Maze maze);
    }
}