namespace Mazes.Contracts
{
    public interface IMazeGenerator<T>
    {
        public Cell<T>?[,] Generate(Cell<T>?[,] cells);
    }
}
