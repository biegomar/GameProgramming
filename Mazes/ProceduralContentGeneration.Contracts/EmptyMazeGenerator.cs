using System.Collections.Generic;
using System.Linq;

namespace Mazes.Contracts
{
    public class EmptyMazeGenerator<T>: IProceduralContentGenerator<T>
    {
        public IList<Cell<T>> Generate(IList<Cell<T>> cells)
        {
            var dimensionZeroLength = cells.Max(cell => cell.X) + 1;
            var dimensionOneLength = cells.Max(cell => cell.Y) + 1;

            IList<Cell<T>> runOfCells = new List<Cell<T>>();
            
            for (int row = dimensionOneLength - 1; row >= 0; row--)
            {
                for (int column = 0; column < dimensionZeroLength; column++)
                {
                    var item = GetCellByColumnAndRow(cells, column, row);
                    
                    if (item.EasternNeighbour != null)
                    {
                        item.LinkCell(item.EasternNeighbour);   
                    }

                    if (item.NorthernNeighbour != null)
                    {
                        item.LinkCell(item.NorthernNeighbour);
                    }
                }
            }

            return cells;
        }
        
        private Cell<T> GetCellByColumnAndRow(IList<Cell<T>> cells, int column, int row)
        {
            return cells.Single(cell => cell.X == column && cell.Y == row);
        }
    }
}