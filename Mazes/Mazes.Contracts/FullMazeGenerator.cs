namespace Mazes.Contracts
{
    public class FullMazeGenerator<T>: IMazeGenerator<T>
    {
        public Cell<T>?[,] Generate(Cell<T>?[,] cells)
        {
            return cells;
        }
    }
}