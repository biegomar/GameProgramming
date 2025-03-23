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
        return this.ApplyRules(playGround, (position.X, position.Y));
    }

    public CellState ApplyRules(IPlayGround playGround, (int X, int Y) position)
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
    
    private (bool left, bool right) GetNeighboursState(IPlayGround playGround, (int X, int Y) position)
    {
        (int X, int Y) left = (position.X - 1, position.Y);
        (int X, int Y) right = (position.X + 1, position.Y);

        return (IsWithinBounds(playGround.Dimension, left.X, left.Y) && playGround[left] != CellState.Empty,
            IsWithinBounds(playGround.Dimension, right.X, right.Y) && playGround[right] != CellState.Empty);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector dimension, int x, int y )
    {
        return x >= 0 && y >= 0 &&
               x < dimension.X &&
               y < dimension.Y;
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