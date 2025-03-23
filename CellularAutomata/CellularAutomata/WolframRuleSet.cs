using System.Runtime.CompilerServices;

namespace CellularAutomata;

public sealed class WolframRuleSet : IRuleSet
{
    private readonly int[] wolframRule = new int[8];
    
    public IDictionary<string, uint> RuleCounter { get; init; } = new Dictionary<string, uint>
    {
        ["CellEmpty"] = 0,
        ["CellAlive"] = 0
    };
    
    public WolframRuleSet(int rule)
    {
        InitializeWolframRule(rule);
    }
    
    public CellState ApplyRules(IPlayGround playGround, Vector position)
    {
        var (leftState, rightState) = GetNeighboursState(playGround, position);
        var cellState = playGround[position] != CellState.Empty;
        
        int ruleIndex = (leftState ? 4 : 0) | (cellState ? 2 : 0) | (rightState ? 1 : 0);

        return wolframRule[ruleIndex] == 1 ? CellState.Solid : CellState.Empty;
    }
    
    public IPlayGround ApplySpawnRules(IPlayGround playGround, Vector spawnPosition, Vector brushSize, double probability = 1)
    {
        return playGround;
    }

    private void InitializeWolframRule(int rule)
    {
        for (int i = 0; i < 8; i++)
        {
            wolframRule[i] = (rule >> i) & 1;
        }
    }
    
    private (bool left, bool right) GetNeighboursState(IPlayGround playGround, Vector position)
    {
        var left = new Vector(position.X - 1, position.Y);
        var right = new Vector(position.X + 1, position.Y);

        return (IsWithinBounds(playGround.Dimension, left) && playGround[left] != CellState.Empty,
            IsWithinBounds(playGround.Dimension, right) && playGround[right] != CellState.Empty);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector dimension, Vector position)
    {
        return position.X >= 0 && position.Y >= 0 &&
               position.X < dimension.X &&
               position.Y < dimension.Y;
    }
    
    // Alle Regeln erzeugen!
    // private void InitializeWolframRules()
    // {
    //     for (int rule = 0; rule < 256; rule++)
    //     {
    //         int[] ruleArray = new int[8];
    //         for (int i = 0; i < 8; i++)
    //         {
    //             ruleArray[i] = (rule >> i) & 1;
    //         }
    //         WolframRules[rule] = ruleArray;
    //     }
    // }
}