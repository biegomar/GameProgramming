using System.Collections.Generic;
using Mazes.Contracts.Cells;

namespace Mazes.Contracts
{
    public interface IProceduralContentGenerator<T>
    {
        public IList<Cell<T>> Generate(IList<Cell<T>> cells);

        public IList<Cell<T>> InitializeCells(IList<Cell<T>> cells, CellVector dimension);

        public IList<Cell<T>> LinkCells(IList<Cell<T>> cells);
    }
}
