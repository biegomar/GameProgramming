namespace Mazes.Contracts
{
    public class FullMazeGenerator<T>: IProceduralContentGenerator<T>
    {
        public Cell<T>?[,] Generate(Cell<T>?[,] cells)
        {
            return cells;
        }
    }
}