using System.Runtime.CompilerServices;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.Wolfram;

public sealed class WolframRuleSet 
{
    
    private readonly int[] wolframRule = new int[8];
    
    public WolframRuleSet(int rule)
    {
        InitializeWolframRule(rule);
    }
    
    public bool ApplyRules(SimplePlayGround playGround, Vector position)
    {
        var (leftState, rightState) = GetNeighboursState(playGround, position);
        var state = playGround.GetState(position);
        
        var ruleIndex = (leftState ? 4 : 0) | (state ? 2 : 0) | (rightState ? 1 : 0);

        return wolframRule[ruleIndex] == 1;
    }

    private void InitializeWolframRule(int rule)
    {
        for (int i = 0; i < 8; i++)
        {
            wolframRule[i] = (rule >> i) & 1;
        }
    }
    
    private (bool left, bool right) GetNeighboursState(SimplePlayGround playGround, Vector position)
    {
        var left = new Vector(position.X - 1, position.Y);
        var right = new Vector(position.X + 1, position.Y);

        return (IsWithinBounds(playGround.Dimension, left) && playGround.GetState(left),
            IsWithinBounds(playGround.Dimension, right) && playGround.GetState(right));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector dimension, Vector position)
    {
        return position.X >= 0 && position.Y >= 0 &&
               position.X < dimension.X &&
               position.Y < dimension.Y;
    }
    
    private static void Swap(ref SimplePlayGround instanceOne, ref SimplePlayGround instanceTwo)
    { 
        (instanceOne, instanceTwo) = (instanceTwo, instanceOne);
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