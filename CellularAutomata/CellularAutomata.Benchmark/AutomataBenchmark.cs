namespace CellularAutomata.Benchmark;

using BenchmarkDotNet.Attributes;

public class AutomataBenchmark
{
    private const int Iterations = 50;
    private static Vector dimension = new Vector(800,600);
    private PlayGround<bool> playGround = new PlayGround<bool>(dimension);
    private PlayGroundArray<bool> playGroundArray = new PlayGroundArray<bool>(dimension);
    private GameOfLifeRuleSet ruleSet = new GameOfLifeRuleSet();
    private GameOfLifeRuleSetArray ruleSetArray = new GameOfLifeRuleSetArray();
    private readonly Automata<bool> automataBool = new (dimension);
    private readonly AutomataArray<bool> automataArrayBool = new (dimension);
    
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
            playGround = automataBool.NextGenerationForLoop(playGround, ruleSet, false);    
        }
    }
    
    [Benchmark]
    public void Run()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = automataBool.NextGeneration(playGround, ruleSet, false);    
        }
    }
    
    [Benchmark]
    public void RunParallel()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = automataBool.NextGenerationParallel(playGround, ruleSet, false, Environment.ProcessorCount);    
        }
    }
    
    [Benchmark]
    public void RunParallelHalf()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = automataBool.NextGenerationParallel(playGround, ruleSet, false,Environment.ProcessorCount / 2);    
        }
    }
    
    [Benchmark]
    public void RunParallel2()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = automataBool.NextGenerationParallel(playGround, ruleSet, false, 2);    
        }
    }
    
    [Benchmark]
    public void RunParallel4()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = automataBool.NextGenerationParallel(playGround, ruleSet, false, 4);    
        }
    }
    
    [Benchmark]
    public void RunParallel8()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = automataBool.NextGenerationParallel(playGround, ruleSet, false, 8);    
        }
    }
    
    [Benchmark]
    public void RunParallel16()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = automataBool.NextGenerationParallel(playGround, ruleSet, false, 16);    
        }
    }
    
    [Benchmark]
    public void RunArray()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = automataArrayBool.NextGeneration(playGroundArray, ruleSetArray, false);    
        }
    }
    
    [Benchmark]
    public void RunParallelArrayHalf()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = automataArrayBool.NextGenerationParallel(playGroundArray, ruleSetArray, false, Environment.ProcessorCount / 2);    
        }
    }
    
    [Benchmark]
    public void RunParallelArray()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = automataArrayBool.NextGenerationParallel(playGroundArray, ruleSetArray, false, Environment.ProcessorCount);    
        }
    }
    
    [Benchmark]
    public void RunParallelArray2()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = automataArrayBool.NextGenerationParallel(playGroundArray, ruleSetArray, false, 2);    
        }
    }
    
    [Benchmark]
    public void RunParallelArray4()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = automataArrayBool.NextGenerationParallel(playGroundArray, ruleSetArray, false, 4);    
        }
    }
    
    [Benchmark]
    public void RunParallelArray8()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = automataArrayBool.NextGenerationParallel(playGroundArray, ruleSetArray, false, 8);    
        }
    }
    
    [Benchmark]
    public void RunParallelArray16()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = automataArrayBool.NextGenerationParallel(playGroundArray, ruleSetArray, false, 16);    
        }
    }
}