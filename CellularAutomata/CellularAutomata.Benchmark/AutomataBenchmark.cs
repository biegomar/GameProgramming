namespace CellularAutomata.Benchmark;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class AutomataBenchmark
{
    private const int Iterations = 10;
    private static Vector dimension = new Vector(800,600,0);
    private Vector screenSize = new Vector(dimension.X + 5, dimension.Y + 5, 0);
    PlayGround<bool> playGround = new PlayGround<bool>(dimension);
    GameOfLifeRuleSet ruleSet = new GameOfLifeRuleSet();
    
    public AutomataBenchmark()
    {
        GameOfLifeInitializer.Randomize(playGround, 0.2);
    }
    
    [Benchmark]
    public void Run()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = Automata<bool>.NextGeneration(playGround, ruleSet);    
        }
    }
    
    [Benchmark]
    public void RunParallel()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = Automata<bool>.NextGenerationParallel(playGround, ruleSet);    
        }
    }
    
    [Benchmark(Baseline = true)]
    public void RunForLoop()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = Automata<bool>.NextGenerationForLoop(playGround, ruleSet);    
        }
    }
}