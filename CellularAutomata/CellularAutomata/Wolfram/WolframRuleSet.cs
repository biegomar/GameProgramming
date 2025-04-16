using System.Runtime.CompilerServices;
using CellularAutomata.Cells;
using CellularAutomata.MaterialFlow;

namespace CellularAutomata.Wolfram;

public sealed class WolframRuleSet 
{
    private readonly int[] wolframRule = new int[8];
    
    public WolframRuleSet(int rule)
    {
        InitializeWolframRule(rule);
    }
    
    public CellBrightness ApplyRules(PlayGround playGround, Vector position)
    {
        var (leftState, rightState) = GetNeighboursState(playGround, position);
        var cellType = playGround.GetCellType(position) != CellType.Empty;
        
        int ruleIndex = (leftState ? 4 : 0) | (cellType ? 2 : 0) | (rightState ? 1 : 0);

        return wolframRule[ruleIndex] == 1 ? CellBrightness.Solid : CellBrightness.Empty;
    }

    public MaterialMovement? ApplyMaterialRules(PlayGround playGround, Vector position)
    {
        var (leftState, rightState) = GetNeighboursState(playGround, position);
        try
        {
            var cellType = playGround.GetCell(position).Type != CellType.Empty;
            var ruleIndex = (leftState ? 4 : 0) | (cellType ? 2 : 0) | (rightState ? 1 : 0);

            return wolframRule[ruleIndex] == 1 
                ? new MaterialMovement(new Material(position, new Cell(CellType.Solid, CellBrightness.Solid)), null) 
                : new MaterialMovement(new Material(position, new Cell(CellType.Empty, CellBrightness.Empty)), null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public PlayGround ApplySpawnRules(PlayGround playGround, Vector spawnPosition, Vector brushSize, double probability = 1)
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
    
    private (bool left, bool right) GetNeighboursState(PlayGround playGround, Vector position)
    {
        var left = new Vector(position.X - 1, position.Y);
        var right = new Vector(position.X + 1, position.Y);

        return (IsWithinBounds(playGround.Dimension, left) && playGround.GetCell(left).Type != CellType.Empty,
            IsWithinBounds(playGround.Dimension, right) && playGround.GetCell(right).Type != CellType.Empty);
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