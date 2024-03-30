namespace Mazes.Contracts;

public interface IPrintSubsystem
{
    void DrawMaze(string header, int column);
    void RedrawMaze();
}