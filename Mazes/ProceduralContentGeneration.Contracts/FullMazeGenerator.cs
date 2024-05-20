using System.Collections.Generic;

namespace Mazes.Contracts
{
    public class FullMazeGenerator<T>: BaseMazeGenerator<T>
    {
        public override IList<Cell<T>> Generate(IList<Cell<T>> cells)
        {
            return cells;
        }
    }
}