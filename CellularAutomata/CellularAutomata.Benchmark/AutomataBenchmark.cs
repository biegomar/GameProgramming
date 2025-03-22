namespace CellularAutomata.Benchmark;

using BenchmarkDotNet.Attributes;

public class AutomataBenchmark
{
    private const int Iterations = 50;
    private static Vector dimension = new Vector(800,600);
    private PlayGround playGround = new PlayGround(dimension);
    private PlayGroundArray playGroundArray = new PlayGroundArray(dimension);
    private GameOfLifeRuleSet ruleSet = new GameOfLifeRuleSet(dimension);
    private GameOfLifeRuleSetArray ruleSetArray = new GameOfLifeRuleSetArray(dimension);
    private readonly Automata automataBool = new (dimension);
    private readonly AutomataArray automataArrayBool = new (dimension);
    
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
            playGround = automataBool.NextGenerationForLoop(playGround, ruleSet, false, new Vector(0,0));    
        }
    }
    
    [Benchmark]
    public void Run()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = automataBool.NextGeneration(playGround, ruleSet, false, new Vector(0,0));    
        }
    }
    
    [Benchmark]
    public void RunParallel()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = automataBool.NextGenerationParallel(playGround, ruleSet, false, new Vector(0,0), Environment.ProcessorCount);    
        }
    }
    
    [Benchmark]
    public void RunParallelHalf()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = automataBool.NextGenerationParallel(playGround, ruleSet, false, new Vector(0,0),Environment.ProcessorCount / 2);    
        }
    }
    
    [Benchmark]
    public void RunParallel2()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = automataBool.NextGenerationParallel(playGround, ruleSet, false, new Vector(0,0), 2);    
        }
    }
    
    [Benchmark]
    public void RunParallel4()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = automataBool.NextGenerationParallel(playGround, ruleSet, false, new Vector(0,0), 4);    
        }
    }
    
    [Benchmark]
    public void RunParallel8()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = automataBool.NextGenerationParallel(playGround, ruleSet, false, new Vector(0,0), 8);    
        }
    }
    
    [Benchmark]
    public void RunParallel16()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGround = automataBool.NextGenerationParallel(playGround, ruleSet, false, new Vector(0,0), 16);    
        }
    }
    
    [Benchmark]
    public void RunArray()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = automataArrayBool.NextGeneration(playGroundArray, ruleSetArray, false, new Vector(0,0));    
        }
    }
    
    [Benchmark]
    public void RunParallelArrayHalf()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = automataArrayBool.NextGenerationParallel(playGroundArray, ruleSetArray, false, new Vector(0,0), Environment.ProcessorCount / 2);    
        }
    }
    
    [Benchmark]
    public void RunParallelArray()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = automataArrayBool.NextGenerationParallel(playGroundArray, ruleSetArray, false, new Vector(0,0), Environment.ProcessorCount);    
        }
    }
    
    [Benchmark]
    public void RunParallelArray2()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = automataArrayBool.NextGenerationParallel(playGroundArray, ruleSetArray, false, new Vector(0,0), 2);    
        }
    }
    
    [Benchmark]
    public void RunParallelArray4()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = automataArrayBool.NextGenerationParallel(playGroundArray, ruleSetArray, false, new Vector(0,0), 4);    
        }
    }
    
    [Benchmark]
    public void RunParallelArray8()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = automataArrayBool.NextGenerationParallel(playGroundArray, ruleSetArray, false, new Vector(0,0), 8);    
        }
    }
    
    [Benchmark]
    public void RunParallelArray16()
    {
        for (int i = 0; i < Iterations; i++)
        {
            playGroundArray = automataArrayBool.NextGenerationParallel(playGroundArray, ruleSetArray, false, new Vector(0,0), 16);    
        }
    }
}