using System.Collections.Generic;

namespace Mazes.Contracts
{
    public abstract class BaseDungeonGenerator<T> : IProceduralContentGenerator<T>
    {
        public abstract IList<Cell<T>> Generate(IList<Cell<T>> cells);

        public IList<Cell<T>> InitializeCells(IList<Cell<T>> cells)
        {
            return cells;
        }

        public IList<Cell<T>> LinkCells(IList<Cell<T>> cells)
        {
            return cells;
        }
    }
}