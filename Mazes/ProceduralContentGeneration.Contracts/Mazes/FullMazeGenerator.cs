using System.Collections.Generic;
using Mazes.Contracts.Cells;

namespace Mazes.Contracts.Mazes
{
    public class FullMazeGenerator<T>: BaseMazeGenerator<T>
    {
        public override IList<Cell<T>> Generate(IList<Cell<T>> cells)
        {
            return cells;
        }
    }
}