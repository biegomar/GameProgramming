using System.Runtime.CompilerServices;

namespace CellularAutomata;

public sealed class Automata
{
    private PlayGround nextGenerationPlayGround;
    
    public Automata(Vector dimension)
    {
        nextGenerationPlayGround = new PlayGround(dimension);
    }
    
    public PlayGround NextGeneration(PlayGround initialPlayGround, IRuleSet ruleSet, bool isSpawn, Vector spawnPosition, Vector brushSize)
    {
        foreach (var cell in initialPlayGround.Cells)
        {
            nextGenerationPlayGround[cell.Key] = ruleSet.ApplyRules(initialPlayGround, cell.Key); 
        }

        if (isSpawn)
        {
            nextGenerationPlayGround = ApplySpawnRules(nextGenerationPlayGround, ruleSet, spawnPosition, brushSize);   
        }
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }

    public PlayGround NextGenerationParallel(PlayGround initialPlayGround, IRuleSet ruleSet, bool isSpawn, Vector spawnPosition, Vector brushSize, int maxDegreeOfParallelism)
    {
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = Math.Min(maxDegreeOfParallelism, Environment.ProcessorCount)
        };

        var ground = initialPlayGround;
        Parallel.ForEach(initialPlayGround.Cells, parallelOptions, cell =>
        {
            nextGenerationPlayGround[cell.Key] = ruleSet.ApplyRules(ground, cell.Key);
        });

        if (isSpawn)
        {
            nextGenerationPlayGround = ApplySpawnRules(nextGenerationPlayGround, ruleSet, spawnPosition, brushSize); 
        }
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    public PlayGround NextGenerationForLoop(PlayGround initialPlayGround, IRuleSet ruleSet, bool isSpawn, Vector spawnPosition, Vector brushSize)
    {
        var dimensionX = initialPlayGround.Dimension.X;
        var dimensionY = initialPlayGround.Dimension.Y;
        
        for (var x = 0; x < dimensionX; x++)
        {
            for (var y = 0; y < dimensionY; y++)
            {
                nextGenerationPlayGround[(x,y)] = ruleSet.ApplyRules(initialPlayGround, new Vector(x,y));
            }
        }

        if (isSpawn)
        {
            nextGenerationPlayGround = ApplySpawnRules(nextGenerationPlayGround, ruleSet, spawnPosition, brushSize);    
        }
        
        Swap(ref initialPlayGround, ref nextGenerationPlayGround);
        
        return initialPlayGround;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PlayGround ApplySpawnRules(PlayGround playGround, IRuleSet ruleSet, Vector spawnPosition, Vector brushSize, double probability = 1)
    {
        return (PlayGround)ruleSet.ApplySpawnRules(playGround, spawnPosition, brushSize, probability); 
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Swap(ref PlayGround instanceOne, ref PlayGround instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
    }
}