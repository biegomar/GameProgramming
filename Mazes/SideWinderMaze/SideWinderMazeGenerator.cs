using Mazes.Contracts;

namespace SideWinderMaze
{
    public class SideWinderMazeGenerator : IMazeGenerator
    {
        public Cell[,] Generate(Cell[,] rawMaze)
        {
            var randomGenerator = new Random();
            var dimensionZeroLength = rawMaze.GetLength(0);
            var dimensionOneLength = rawMaze.GetLength(1);

            IList<Cell> runOfCells = new List<Cell>();
            
            for (int row = dimensionOneLength - 1; row >= 0; row--)
            {
                for (int column = 0; column < dimensionZeroLength; column++)
                {
                    var item = rawMaze[column, row];

                    runOfCells.Add(item);
                    
                    if (item.WesternNeighbour != null && runOfCells.Contains(item.WesternNeighbour))
                    {
                        item.LinkCell(item.WesternNeighbour);   
                    }

                    //var choiceList = new List<Cell>();
                    //if (item.EasternNeighbour != null)
                    //{
                    //    choiceList.Add(item.EasternNeighbour!);
                    //}
                    //if (item.NothernNeighbour != null)
                    //{
                    //    choiceList.Add(item.NothernNeighbour!);
                    //}                  

                    //if (choiceList.Count == 1)
                    //{
                    //    choiceList.First().LinkCell(item);
                    //}
                    //else if (choiceList.Count == 2)
                    //{
                    //    var choice = randomGenerator.Next(0, 2);
                    //    if (choice == 1)
                    //    {
                    //        choiceList.First().LinkCell(item);
                    //    }
                    //    else
                    //    {
                    //        choiceList.Last().LinkCell(item);
                    //    }
                    //}
                    var choice = randomGenerator.Next(0, 2);
                    
                    if (choice == 1 && item.NothernNeighbour != null || item.EasternNeighbour == null)
                    {
                        var choiceNorth = randomGenerator.Next(0, runOfCells.Count);
                        var itemToLink = runOfCells[choiceNorth];
                        if (itemToLink.NothernNeighbour != null)
                        {
                            itemToLink.LinkCell(itemToLink.NothernNeighbour!);
                        }
                        runOfCells.Clear();
                    }
                }
            }

            return rawMaze;
        }
    }
}
