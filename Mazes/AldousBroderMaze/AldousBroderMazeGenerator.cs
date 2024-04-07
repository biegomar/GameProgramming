using Mazes.Contracts;

namespace AldousBroderMaze;

public class AldousBroderMazeGenerator<T>(IMazePrinter<T>? mazePrinter, int numberOfAgents=1) : IMazeGenerator<T>
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
    
    public Cell<T>?[,] Generate(Cell<T>?[,] cells)
    {
        var dimensionZeroLength = cells.GetLength(0);
        var dimensionOneLength = cells.GetLength(1);

        List<Agent> agents = new List<Agent>();
        // for (int i = 0; i < numberOfAgents; i++)
        // {
        //     var agent = new Agent<T>(randomGenerator.Next(0, dimensionZeroLength),
        //         randomGenerator.Next(0, dimensionOneLength));
        //     agent.ActualCell = cells[agent.StartPositionX, agent.StartPositionY];
        //     agent.Item = Convert.ToChar(i.ToString());
        //     
        //     agents.Add(agent);
        // }

        {
            Agent agent = new Agent(0,0);
            agent.ActualCell = cells[0, 0];
            agent.Item =
                mazePrinter != null && mazePrinter.Items != null && mazePrinter.Items.Any() &&
                mazePrinter.Items[0] != null
                    ? mazePrinter.Items[0]
                    : default; 
            agents.Add(agent);
            
            Agent agent1 = new Agent(9,9);
            agent1.ActualCell = cells[9, 9];
            agent1.Item =
                mazePrinter != null && mazePrinter.Items != null && mazePrinter.Items.Any() &&
                mazePrinter.Items[1] != null
                    ? mazePrinter.Items[1]
                    : default; 
            agents.Add(agent1);
            
            Agent agent2 = new Agent(0,9);
            agent2.ActualCell = cells[0, 9];
            agent2.Item = 
                mazePrinter != null && mazePrinter.Items != null && mazePrinter.Items.Any() &&
                mazePrinter.Items[2] != null
                    ? mazePrinter.Items[2]
                    : default; 
            agents.Add(agent2);
            
            Agent agent3 = new Agent(9,0);
            agent3.ActualCell = cells[9, 0];
            agent3.Item =
                mazePrinter != null && mazePrinter.Items != null && mazePrinter.Items.Any() &&
                mazePrinter.Items[3] != null
                    ? mazePrinter.Items[3]
                    : default; 
            agents.Add(agent3);
            
            Agent agent4 = new Agent(5,5);
            agent4.ActualCell = cells[5, 5];
            agent4.Item =
                mazePrinter != null && mazePrinter.Items != null && mazePrinter.Items.Any() &&
                mazePrinter.Items[4] != null
                    ? mazePrinter.Items[4]
                    : default; 
            agents.Add(agent4);
        }
        
        countOfCells = cells.Length - agents.Count;

        stepCounter = 0;
            
        do
        {
            foreach (var agent in agents)
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
                
                PrintDuringGeneration(cells, agents, agent);
            
                agent.ActualCell = agent.NextCell;
            }

            stepCounter++;
        } while (countOfCells > 0);

        return cells;
    }

    private void PrintDuringGeneration(Cell<T>?[,] rawMaze, List<Agent> agents, Agent actualAgent)
    {
        if (mazePrinter != null)
        {
            mazePrinter.DrawMaze(rawMaze, new MazeVector(0,0,0), $"AldousBroder Cells left: {countOfCells} / steps: {stepCounter}      ");

            foreach (var agent in agents)
            {
                if (agent.Item != null)
                {
                    mazePrinter.DrawItemAtPosition(rawMaze, new MazeVector(agent.ActualCell.X, agent.ActualCell.Y, 0),
                        agent.Item);
                }
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

    private Cell<T?> GetNextCellCandidate(Cell<T?> cell)
    {
        var allNeighbours = this.GetAllNeighbours(cell);

        var result = allNeighbours[this.randomGenerator.Next(0, allNeighbours.Length)];

        return result;
    }
}
