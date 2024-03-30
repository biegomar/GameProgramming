namespace Mazes.Contracts;

public interface IMazePrinter
{
    void DrawMaze(MazeVector startMazeVector);
    void RedrawMaze();
}