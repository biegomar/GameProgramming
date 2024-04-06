using Mazes.Contracts;

namespace AldousBroderMaze;

public class AldousBroderMazeGenerator(IMazePrinter? mazePrinter) : IMazeGenerator
{
    private Random randomGenerator = new();
    private int countOfCells;
    
    public Cell<T>[,] Generate<T>(Cell<T>[,] cells)
    {
        var dimensionZeroLength = cells.GetLength(0);
        var dimensionOneLength = cells.GetLength(1);

        var agentStartPositionX = randomGenerator.Next(0, dimensionZeroLength);
        var agentStartPositionY = randomGenerator.Next(0, dimensionOneLength);
        countOfCells = cells.Length - 1;
        var actualCell = cells[agentStartPositionX, agentStartPositionY];

        do
        {
            var nextCellCandidate = this.GetNextCellCandidate(actualCell);
            
            PrintDuringGeneration(cells, actualCell, nextCellCandidate);
            
            if (!actualCell.LinkedCells.Contains(nextCellCandidate))
            {
                if (!nextCellCandidate.LinkedCells.Any())
                {
                    countOfCells--;
                    actualCell.LinkCell(nextCellCandidate);
                }
            }
            
            actualCell = nextCellCandidate;
            
        } while (countOfCells > 0);

        return cells;
    }

    private void PrintDuringGeneration<T>(Cell<T>[,] rawMaze, Cell<T> actualCell, Cell<T> nextCell)
    {
        if (mazePrinter != null)
        {
            mazePrinter.DrawMaze(rawMaze, new MazeVector(0,0,0), $"AldousBroder {countOfCells}");
            Console.SetCursorPosition(2 + actualCell.X * 4, (actualCell.Y + 2) * 2);
            Console.Write('I');
            Console.ReadKey();
        }
        
        nextCell.Item = actualCell.Item;
        actualCell.Item = default!;
    }

    private Cell<T>[] GetAllNeighbours<T>(Cell<T> cell)
    {
        var result = new List<Cell<T>>();

        if (cell.EasternNeighbour != null)
        {
            result.Add(cell.EasternNeighbour);
        }
        
        if (cell.WesternNeighbour != null)
        {
            result.Add(cell.WesternNeighbour);
        }
        
        if (cell.NothernNeighbour != null)
        {
            result.Add(cell.NothernNeighbour);
        }
        
        if (cell.SouthernNeighbour != null)
        {
            result.Add(cell.SouthernNeighbour);
        }

        return result.ToArray();
    }

    private Cell<T> GetNextCellCandidate<T>(Cell<T> cell)
    {
        var allNeighbours = this.GetAllNeighbours(cell);

        var result = allNeighbours[this.randomGenerator.Next(0, allNeighbours.Length)];

        return result;
    }
}
