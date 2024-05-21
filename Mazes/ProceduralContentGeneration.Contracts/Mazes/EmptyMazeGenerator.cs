using System.Collections.Generic;
using System.Linq;
using Mazes.Contracts.Cells;

namespace Mazes.Contracts.Mazes
{
    public class EmptyMazeGenerator<T>: BaseMazeGenerator<T>
    {
        public override IList<Cell<T>> Generate(IList<Cell<T>> cells)
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
    }
}