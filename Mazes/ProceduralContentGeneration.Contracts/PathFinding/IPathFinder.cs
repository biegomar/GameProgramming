using System.Collections.Generic;

namespace Mazes.Contracts.PathFinding
{
    public interface IPathFinder<T>
    {
        public IList<MazeVector> GetShortestPath(MazeVector startPoint, MazeVector endPoint);
    }
}