namespace Mazes.Contracts
{
    public interface IProceduralContentGenerator<T>
    {
        public Cell<T>?[,] Generate(Cell<T>?[,] cells);
    }
}
