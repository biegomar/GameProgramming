using System.Collections.Generic;
using Mazes.Contracts.Cells;

namespace Mazes.Contracts.PathFinding
{
    public interface IPathFinder<T>
    {
        public IList<CellVector> GetShortestPath(CellVector startPoint, CellVector endPoint);
    }
}