using Mazes.Contracts;

namespace AldousBroderMaze;

public class AldousBroderMazeGenerator<T>(IContentPrinter<T>? mazePrinter) : IProceduralContentGenerator<T>
{
    private Random randomGenerator = new Random();
    private int countOfCells;
    private Int64 stepCounter;

    private record Agent()
    {
        public int StartPositionX { get; set; }
        public int StartPositionY { get; set; }

        public T? Item { get; set; }

        public Cell<T> ActualCell { get; set; }
        
        public Cell<T> NextCell { get; set; }

        public Agent(int startPositionX, int startPositionY): this()
        {
            this.ActualCell = new Cell<T>();
            this.NextCell = new Cell<T>();
            this.StartPositionX = startPositionX;
            this.StartPositionY = startPositionY;
        }
    }
    
    public IList<Cell<T>> Generate(IList<Cell<T>> cells)
    {
        var dimensionZeroLength = cells.Max(cell => cell.X) + 1;
        var dimensionOneLength = cells.Max(cell => cell.Y) + 1;

        Agent agent = new Agent(randomGenerator.Next(0, dimensionZeroLength),
            randomGenerator.Next(0, dimensionOneLength));
        agent.ActualCell = GetCellByColumnAndRow(cells, agent.StartPositionX, agent.StartPositionY);
        agent.Item =
            mazePrinter != null && mazePrinter.Items != null && mazePrinter.Items.Any() &&
            mazePrinter.Items[4] != null
                ? mazePrinter.Items[4]
                : default; 
        
        countOfCells = cells.Count - 1;

        stepCounter = 0;
            
        do
        {
            agent.NextCell = this.GetNextCellCandidate(agent.ActualCell);
                
            if (!agent.ActualCell.LinkedCells.Contains(agent.NextCell))
            {
                if (!agent.NextCell.LinkedCells.Any())
                {
                    countOfCells--;
                    agent.ActualCell.LinkCell(agent.NextCell);
                }
            }
                
            PrintDuringGeneration(cells, agent);
            
            agent.ActualCell = agent.NextCell;

            stepCounter++;
        } while (countOfCells > 0);

        return cells;
    }

    private void PrintDuringGeneration(IList<Cell<T>> rawMaze, Agent actualAgent)
    {
        if (mazePrinter != null)
        {
            mazePrinter.DrawCells(rawMaze, CellVector.Zero, $"AldousBroder Cells left: {countOfCells} / steps: {stepCounter}      ");

            if (actualAgent.Item != null)
            {
                mazePrinter.DrawItemAtPosition(rawMaze, new CellVector(actualAgent.ActualCell.X, actualAgent.ActualCell.Y, 0),
                    actualAgent.Item);
            }
            
            Thread.Sleep(10);   
        }
        
        actualAgent.NextCell.Item = actualAgent.ActualCell.Item;
        actualAgent.ActualCell.Item = default!;
    }

    private Cell<T?>[] GetAllNeighbours(Cell<T?> cell)
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
        
        if (cell.NorthernNeighbour != null)
        {
            result.Add(cell.NorthernNeighbour);
        }
        
        if (cell.SouthernNeighbour != null)
        {
            result.Add(cell.SouthernNeighbour);
        }

        return result.ToArray();
    }

    private Cell<T?> GetNextCellCandidate(Cell<T?> cell)
    {
        var allNeighbours = this.GetAllNeighbours(cell);

        var result = allNeighbours[this.randomGenerator.Next(0, allNeighbours.Length)];

        return result;
    }
    
    private Cell<T> GetCellByColumnAndRow(IList<Cell<T>> cells, int column, int row)
    {
        return cells.Single(cell => cell.X == column && cell.Y == row);
    }
}
