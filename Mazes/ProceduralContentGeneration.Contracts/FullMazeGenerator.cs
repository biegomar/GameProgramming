using System.Collections.Generic;

namespace Mazes.Contracts
{
    public class FullMazeGenerator<T>: IProceduralContentGenerator<T>
    {
        public IList<Cell<T>> Generate(IList<Cell<T>> cells)
        {
            return cells;
        }
    }
}