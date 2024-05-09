using System.Collections.Generic;
using Mazes.Contracts;
using Mazes.Contracts.PathFinding;

namespace MazePathFinder
{
    public class PathFinderForMaze<T> : IPathFinder<T>
    {
        private readonly Maze<T> maze;
        private readonly Cell<T>?[,] Cells;
        
        public PathFinderForMaze(Maze<T> maze)
        {
            this.maze = maze;
            this.Cells = maze.Cells;
        }

        public IList<MazeVector> GetShortestPath(MazeVector startPoint, MazeVector endPoint)
        {
            var startCell = Cells[startPoint.X, startPoint.Y]!;
            var endCell = Cells[endPoint.X, endPoint.Y]!;
            var queue = new Queue<Cell<T>>();
            queue.Enqueue(startCell);
            startCell.IsVisited = true;
            startCell.PathCount = 0;

            while (queue.Count > 0)
            {
                var currentCell = queue.Dequeue();

                if (currentCell == endCell)
                {
                    return ReconstructPath(startCell, endCell);
                }

                foreach (var linkedCell in GetUnvisitedLinkedCells(currentCell))
                {
                    linkedCell.IsVisited = true;
                    linkedCell.Predecessor = currentCell;
                    queue.Enqueue(linkedCell);
                }
            }

            return new List<MazeVector>();
        }
        
        private List<MazeVector> ReconstructPath(Cell<T> start, Cell<T> end)
        {
            var path = new List<MazeVector>();
            var currentCell = end;

            while (currentCell != start)
            {
                path.Add(new MazeVector(currentCell.X, currentCell.Y, currentCell.PathCount));
                currentCell = currentCell.Predecessor!;
            }
            
            path.Add(new MazeVector(start.X, start.Y, 0));

            path.Reverse();
            return path;
        }
        
        private IEnumerable<Cell<T>> GetUnvisitedLinkedCells(Cell<T> cell)
        {
            var linkedCells = new List<Cell<T>>();
            var count = cell.PathCount + 1;

            foreach (var linkedCell in cell.LinkedCells)
            {
                if (linkedCell is Cell<T> {IsVisited: false} unvisitedLinkedCell)
                {
                    unvisitedLinkedCell.PathCount = count;
                    linkedCells.Add(unvisitedLinkedCell);
                }
            }

            return linkedCells;
        }
    }
}