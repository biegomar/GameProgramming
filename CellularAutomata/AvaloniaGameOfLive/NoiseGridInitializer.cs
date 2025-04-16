using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CellularAutomata;
using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace AvaloniaGameOfLive;

public static class NoiseGridInitializer
{
    public static void Randomize(SimplePlayGround playground, double density = 0.65)
    {
        var random = new Random();

        for (int x = 0; x < playground.Dimension.X; x++)
        {
            for (int y = 0; y < playground.Dimension.Y; y++)
            {
                var state = random.NextDouble() < density;
                playground.SetState(new Vector(x, y), state);
            }
        }
    } 
    
    public static void AddCheckerboard(SimplePlayGround playground)
    {
        for (int y = 0; y < playground.Dimension.Y; y++)
        {
            AddCheckerLine(playground, y);
        }
    }
    
    private static void AddCheckerLine(SimplePlayGround playground, int row)
    {
        for (int x = 0; x < playground.Dimension.X; x++)
        {
            playground.SetState(new Vector(x, row), int.IsEvenInteger(x) && int.IsEvenInteger(row) || int.IsOddInteger(x) && int.IsOddInteger(row));
        } 
    }
}