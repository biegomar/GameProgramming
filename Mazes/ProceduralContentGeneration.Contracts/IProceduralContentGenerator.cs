using System.Collections.Generic;

namespace Mazes.Contracts
{
    public interface IProceduralContentGenerator<T>
    {
        public IList<Cell<T>> Generate(IList<Cell<T>> cells);

        public IList<Cell<T>> InitializeCells(IList<Cell<T>> cells);

        public IList<Cell<T>> LinkCells(IList<Cell<T>> cells);
    }
}
