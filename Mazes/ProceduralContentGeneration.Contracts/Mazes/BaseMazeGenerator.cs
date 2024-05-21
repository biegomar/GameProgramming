using System.Collections.Generic;
using System.Linq;
using Mazes.Contracts.Cells;

namespace Mazes.Contracts.Mazes
{
    public abstract class BaseMazeGenerator<T> : IProceduralContentGenerator<T>
    {
        public abstract IList<Cell<T>> Generate(IList<Cell<T>> cells);

        public IList<Cell<T>> InitializeCells(IList<Cell<T>> cells)
        {
            var width = cells.Max(cell => cell.X) + 1;
            var height = cells.Max(cell => cell.Y) + 1;
            
            for (int column = 0; column < width; column++)           
            {
                for(int row = 0; row < height; row++)
                {
                    
                    cells.Add(new Cell<T>(column, row));
                }
            }

            return cells;
        }

        public IList<Cell<T>> LinkCells(IList<Cell<T>> cells)
        {
            var width = cells.Max(cell => cell.X) + 1;
            var height = cells.Max(cell => cell.Y) + 1;
            
            for (int column = 0; column < width; column++)
            {
                for (int row = 0; row < height; row++)
                {
                    var cellToLink = GetCellByColumnAndRow(cells, column, row);
                    cellToLink.NorthernNeighbour = row - 1 < 0 ? null : GetCellByColumnAndRow(cells, column, row - 1);
                    cellToLink.EasternNeighbour = column + 1 >= width ? null : GetCellByColumnAndRow(cells, column + 1, row);
                    cellToLink.SouthernNeighbour = row + 1 >= height ? null : GetCellByColumnAndRow(cells, column, row + 1);
                    cellToLink.WesternNeighbour = column - 1 < 0 ? null : GetCellByColumnAndRow(cells, column - 1, row);
                }
            }

            return cells;
        }
        protected Cell<T> GetCellByColumnAndRow(IList<Cell<T>> cells, int column, int row)
        {
            return cells.Single(cell => cell.X == column && cell.Y == row);
        }
        
    }
}