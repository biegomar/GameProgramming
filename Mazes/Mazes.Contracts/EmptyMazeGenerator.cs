namespace Mazes.Contracts;

public class EmptyMazeGenerator: IMazeGenerator
{
    public Cell[,] Generate(Cell[,] rawMaze)
    {
        var dimensionZeroLength = rawMaze.GetLength(0);
        var dimensionOneLength = rawMaze.GetLength(1);

        IList<Cell> runOfCells = new List<Cell>();
            
        for (int row = dimensionOneLength - 1; row >= 0; row--)
        {
            for (int column = 0; column < dimensionZeroLength; column++)
            {
                var item = rawMaze[column, row];
                    
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

        return rawMaze;
    }
}