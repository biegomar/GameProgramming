namespace Mazes.Contracts
{
    public class FullMazeGenerator: IMazeGenerator
    {
        public Cell<T>[,] Generate<T>(Cell<T>[,] rawMaze)
        {
            return rawMaze;
        }
    }
}