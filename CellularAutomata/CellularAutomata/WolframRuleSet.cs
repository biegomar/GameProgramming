using System.Runtime.CompilerServices;

namespace CellularAutomata;

public sealed class WolframRuleSet : IRuleSet<bool> 
{
    //private readonly Dictionary<int, int[]> WolframRules = new Dictionary<int, int[]>();
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
    
    public bool ApplyRules(IPlayGround<bool> playGround, Vector position)
    {
        return this.ApplyRules(playGround, (position.X, position.Y));
    }

    public bool ApplyRules(IPlayGround<bool> playGround, (int X, int Y) position)
    {
        var (leftState, rightState) = GetNeighboursState(playGround, position);
        var cellState = playGround[position];
        
        int ruleIndex = (leftState ? 4 : 0) | (cellState ? 2 : 0) | (rightState ? 1 : 0);

        return wolframRule[ruleIndex] == 1;
    }

    public IPlayGround<bool> ApplySpawnRules(IPlayGround<bool> playGround, bool isSpawn)
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
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private (bool left, bool right) GetNeighboursState(IPlayGround<bool> playGround, (int X, int Y) position)
    {
        (int X, int Y) left = (position.X - 1, position.Y);
        (int X, int Y) right = (position.X + 1, position.Y);

        return (IsWithinBounds(playGround.Dimension, left.X, left.Y) && playGround[left],
            IsWithinBounds(playGround.Dimension, right.X, right.Y) && playGround[right]);
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