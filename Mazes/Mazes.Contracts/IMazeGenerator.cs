namespace Mazes.Contracts
{
    public interface IMazeGenerator
    {
        public Cell<T>[,] Generate<T>(Cell<T>[,] cells);
    }
}
