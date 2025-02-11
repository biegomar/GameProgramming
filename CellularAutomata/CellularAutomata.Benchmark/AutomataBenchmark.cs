namespace CellularAutomata.Benchmark;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class AutomataBenchmark
{
    private const int Iterations = 10;
    private static Vector dimension = new Vector(800,600,0);
    private Vector screenSize = new Vector(dimension.X + 5, dimension.Y + 5, 0);
    PlayGround<bool> playGround = new PlayGround<bool>(dimension);
    PlayGroundArray<bool> playGroundArray = new PlayGroundArray<bool>(dimension);
    GameOfLifeRuleSet ruleSet = new GameOfLifeRuleSet();
    GameOfLifeRuleSetArray ruleSetArray = new GameOfLifeRuleSetArray();
    
    public AutomataBenchmark()
    {
        GameOfLifeInitializer.Randomize(playGround, 0.2);
        GameOfLifeInitializer.Randomize(playGroundArray, 0.2);
    }
    
    [Benchmark(Baseline = true)]
    public void RunBaseline()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = Automata<bool>.NextGenerationForLoop(playGround, ruleSet, false);    
        }
    }
    
    [Benchmark]
    public void Run()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = Automata<bool>.NextGeneration(playGround, ruleSet, false);    
        }
    }
    
    [Benchmark]
    public void RunParallel()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = Automata<bool>.NextGenerationParallel(playGround, ruleSet, false);    
        }
    }
    
    [Benchmark]
    public void RunArray()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = AutomataArray<bool>.NextGeneration(playGroundArray, ruleSetArray, false);    
        }
    }
    
    [Benchmark]
    public void RunParallelArray()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = AutomataArray<bool>.NextGenerationParallel(playGroundArray, ruleSetArray, false);    
        }
    }
}