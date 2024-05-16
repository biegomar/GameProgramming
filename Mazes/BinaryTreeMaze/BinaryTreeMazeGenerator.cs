using System;
using System.Collections.Generic;
using System.Linq;
using Mazes.Contracts;

namespace BinaryTreeMaze
{
    public class BinaryTreeMazeGenerator<T> : IProceduralContentGenerator<T>
    {
        public Cell<T>?[,] Generate(Cell<T>?[,] cells)
        {
            var randomGenerator = new Random();
            var dimensionZeroLength = cells.GetLength(0);
            var dimensionOneLength = cells.GetLength(1);

            for (int column = 0; column < dimensionZeroLength; column++)
            {
                for (int row = 0; row < dimensionOneLength; row++)
                {
                    var item = cells[column, row];

                    var choiceList = new List<Cell<T>?>();
                    if (item.NorthernNeighbour != null)
                    {
                        choiceList.Add(item.NorthernNeighbour!);
                    }
                    if (item.EasternNeighbour != null)
                    {
                        choiceList.Add(item.EasternNeighbour!);
                    }

                    if (choiceList.Count == 1)
                    {
                        choiceList.First().LinkCell(item);
                    }
                    else if (choiceList.Count == 2)
                    {
                        var choice = randomGenerator.Next(0, 2);
                        if (choice == 1)
                        {
                            choiceList.First().LinkCell(item);
                        }
                        else
                        {
                            choiceList.Last().LinkCell(item);
                        }
                    }
                }
            }

            return cells;
        }
    }
}