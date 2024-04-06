using System.Collections.Generic;

namespace Mazes.Contracts
{
    public class EmptyMazeGenerator: IMazeGenerator
    {
        public Cell<T>[,] Generate<T>(Cell<T>[,] cells)
        {
            var dimensionZeroLength = cells.GetLength(0);
            var dimensionOneLength = cells.GetLength(1);

            IList<Cell<T>> runOfCells = new List<Cell<T>>();
            
            for (int row = dimensionOneLength - 1; row >= 0; row--)
            {
                for (int column = 0; column < dimensionZeroLength; column++)
                {
                    var item = cells[column, row];
                    
                    if (item.EasternNeighbour != null)
                    {
                        item.LinkCell(item.EasternNeighbour);   
                    }

                    if (item.NothernNeighbour != null)
                    {
                        item.LinkCell(item.NothernNeighbour);
                    }
                }
            }

            return cells;
        }
    }
}