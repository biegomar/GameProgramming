using System.Collections.Generic;

namespace Mazes.Contracts.PathFinding
{
    public interface IPathFinder<T>
    {
        public IList<CellVector> GetShortestPath(CellVector startPoint, CellVector endPoint);
    }
}