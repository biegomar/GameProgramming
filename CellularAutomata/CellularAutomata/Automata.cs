using System.Runtime.CompilerServices;

namespace CellularAutomata;

public sealed class Automata
{
    private PlayGround? nextGenerationPlayGround;
    
    private void InitNextGenerationPlayGround(Vector dimension)
    {
        if (nextGenerationPlayGround == null || nextGenerationPlayGround.Dimension != dimension)
        {
            nextGenerationPlayGround = new PlayGround(dimension);
        }
    }

    public Automata(Vector dimension)
    {
        this.InitNextGenerationPlayGround(dimension);
    }
    
    public PlayGround NextGeneration(PlayGround initialPlayGround, IRuleSet ruleSet, bool isSpawn)
    {
        //InitNextGenerationPlayGround(initialPlayGround);
        
        foreach (var cell in initialPlayGround.Cells)
        {
            nextGenerationPlayGround![cell.Key] = ruleSet.ApplyRules(initialPlayGround, cell.Key); 
        }
        
        nextGenerationPlayGround = (PlayGround)ruleSet.ApplySpawnRules(nextGenerationPlayGround!, isSpawn);
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }

    public PlayGround NextGenerationParallel(PlayGround initialPlayGround, IRuleSet ruleSet, bool isSpawn, int maxDegreeOfParallelism)
    {
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = Math.Min(maxDegreeOfParallelism, Environment.ProcessorCount)
        };

        var ground = initialPlayGround;
        Parallel.ForEach(initialPlayGround.Cells, parallelOptions, cell =>
        {
            nextGenerationPlayGround![cell.Key] = ruleSet.ApplyRules(ground, cell.Key);
        });
        
        nextGenerationPlayGround = (PlayGround)ruleSet.ApplySpawnRules(nextGenerationPlayGround!, isSpawn);
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    public PlayGround NextGenerationForLoop(PlayGround initialPlayGround, IRuleSet ruleSet, bool isSpawn)
    {
        var newPlayGround = new PlayGround(initialPlayGround.Dimension);
        var dimensionX = initialPlayGround.Dimension.X;
        var dimensionY = initialPlayGround.Dimension.Y;
        
        for (var x = 0; x < dimensionX; x++)
        {
            for (var y = 0; y < dimensionY; y++)
            {
                newPlayGround[(x,y)] = ruleSet.ApplyRules(initialPlayGround, (x,y));
            }
        }
        
        var resultPlayGround = (PlayGround)ruleSet.ApplySpawnRules(newPlayGround, isSpawn);
        
        return resultPlayGround;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Swap(ref PlayGround instanceOne, ref PlayGround instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }
}