using Mazes.Contracts;

namespace AldousBroderMaze;

public class AldousBroderMazeGenerator(IMazePrinter? mazePrinter, int numberOfAgents=1) : IMazeGenerator
{
    private Random randomGenerator = new Random();
    private int countOfCells;
    private Int64 stepCounter;

    private record Agent<T>()
    {
        public int StartPositionX { get; set; }
        public int StartPositionY { get; set; }

        public char Item { get; set; }

        public Cell<T> ActualCell { get; set; }
        
        public Cell<T> NextCell { get; set; }

        public Agent(int startPositionX, int startPositionY): this()
        {
            this.StartPositionX = startPositionX;
            this.StartPositionY = startPositionY;
        }
    }
    
    public Cell<T>[,] Generate<T>(Cell<T>[,] cells)
    {
        var dimensionZeroLength = cells.GetLength(0);
        var dimensionOneLength = cells.GetLength(1);

        List<Agent<T>> agents = new List<Agent<T>>();
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
            var agent = new Agent<T>(0,0);
            agent.ActualCell = cells[0, 0];
            agent.Item = '0';
            agents.Add(agent);
            var agent1 = new Agent<T>(9,9);
            agent1.ActualCell = cells[9, 9];
            agent1.Item = '1';
            agents.Add(agent1);
            var agent2 = new Agent<T>(0,9);
            agent2.ActualCell = cells[0, 9];
            agent2.Item = '2';
            agents.Add(agent2);
            var agent3 = new Agent<T>(9,0);
            agent3.ActualCell = cells[9, 0];
            agent3.Item = '3';
            agents.Add(agent3);
            var agent4 = new Agent<T>(5,5);
            agent4.ActualCell = cells[5, 5];
            agent4.Item = '4';
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

    private void PrintDuringGeneration<T>(Cell<T>[,] rawMaze, List<Agent<T>> agents, Agent<T> actualAgent)
    {
        int oldX = Console.CursorLeft;
        int oldY = Console.CursorTop;
        
        if (mazePrinter != null)
        {
            mazePrinter.DrawMaze(rawMaze, new MazeVector(0,0,0), $"AldousBroder Cells left: {countOfCells} / steps: {stepCounter}      ");

            foreach (var agent in agents)
            {
                Console.SetCursorPosition(2 + agent.ActualCell.X * 4, (agent.ActualCell.Y + 2) * 2);
                Console.Write(agent.Item);
            }
            Thread.Sleep(10);   
        }
        
        actualAgent.NextCell.Item = actualAgent.ActualCell.Item;
        actualAgent.ActualCell.Item = default!;
        
        Console.SetCursorPosition(oldX, oldY);
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
