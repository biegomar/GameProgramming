using Mazes.Contracts;

namespace BinareTreeMaze
{
    public class BinareTreeMazeGenerator : IMazeGenerator
    {
        public Cell[,] Generate(Cell[,] rawMaze)
        {
            var randomGenerator = new Random();
            var dimensionZeroLength = rawMaze.GetLength(0);
            var dimensionOneLength = rawMaze.GetLength(1);

            for (int column = 0; column < dimensionZeroLength; column++)
            {
                for (int row = 0; row < dimensionOneLength; row++)
                {
                    var item = rawMaze[column, row];

                    var choiceList = new List<Cell>();
                    if (item.NothernNeighbour != null)
                    {
                        choiceList.Add(item.NothernNeighbour!);
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

            return rawMaze;
        }
    }
}