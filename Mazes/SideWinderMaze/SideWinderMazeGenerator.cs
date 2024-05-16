using System;
using System.Collections.Generic;
using Mazes.Contracts;

namespace SideWinderMaze
{
    public class SideWinderMazeGenerator<T> : IProceduralContentGenerator<T>
    {
        public Cell<T>?[,] Generate(Cell<T>?[,] cells)
        {
            var randomGenerator = new Random();
            var dimensionZeroLength = cells.GetLength(0);
            var dimensionOneLength = cells.GetLength(1);

            IList<Cell<T>> runOfCells = new List<Cell<T>>();
            
            for (int row = dimensionOneLength - 1; row >= 0; row--)
            {
                for (int column = 0; column < dimensionZeroLength; column++)
                {
                    var item = cells[column, row];

                    runOfCells.Add(item);
                    
                    if (item.WesternNeighbour != null && runOfCells.Contains(item.WesternNeighbour))
                    {
                        item.LinkCell(item.WesternNeighbour);   
                    }
                    
                    var choice = randomGenerator.Next(0, 2);
                    
                    if (choice == 1 && item.NorthernNeighbour != null || item.EasternNeighbour == null)
                    {
                        var choiceNorth = randomGenerator.Next(0, runOfCells.Count);
                        var itemToLink = runOfCells[choiceNorth];
                        if (itemToLink.NorthernNeighbour != null)
                        {
                            itemToLink.LinkCell(itemToLink.NorthernNeighbour!);
                        }
                        runOfCells.Clear();
                    }
                }
            }

            return cells;
        }
    }
}
