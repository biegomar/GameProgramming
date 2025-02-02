namespace CellularAutomata.Benchmark;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class AutomataBenchmark
{
    private static Vector dimension = new Vector(100,40,0);
    private Vector screenSize = new Vector(dimension.X + 5, dimension.Y + 5, 0);
    PlayGround<bool> playGround = new PlayGround<bool>(dimension);
    GameOfLifeRuleSet ruleSet = new GameOfLifeRuleSet();
    
    public void Initialize()
    {
        GameOfLifeInitializer.Randomize(playGround, 0.2);
    }
    
    [Benchmark]
    public void Run()
    {
        playGround = Automata<bool>.NextGeneration(playGround, ruleSet);
    }
}