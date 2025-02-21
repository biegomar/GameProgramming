namespace CellularAutomata.Benchmark;

public class GameOfLifeInitializer
{
    public static void Randomize(PlayGround<bool> playground, double aliveProbability = 0.2)
    {
        var random = new Random();

        for (int x = 0; x < playground.Dimension.X; x++)
        {
            for (int y = 0; y < playground.Dimension.Y; y++)
            {
                playground[new Vector(x, y)] = random.NextDouble() < aliveProbability;
            }
        }
    }
    
    public static void Randomize(PlayGroundArray<bool> playground, double aliveProbability = 0.2)
    {
        var random = new Random();

        for (int x = 0; x < playground.Dimension.X; x++)
        {
            for (int y = 0; y < playground.Dimension.Y; y++)
            {
                playground[(x, y)] = random.NextDouble() < aliveProbability;
            }
        }
    }
}