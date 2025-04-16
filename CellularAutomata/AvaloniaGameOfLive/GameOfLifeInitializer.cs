using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using CellularAutomata;
using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace AvaloniaGameOfLive;

public static class GameOfLifeInitializer
{
    public static void Randomize(SimplePlayGround playground, double aliveProbability = 0.2)
    {
        var random = new Random();

        for (int x = 0; x < playground.Dimension.X; x++)
        {
            for (int y = 0; y < playground.Dimension.Y; y++)
            {
                var state = random.NextDouble() < aliveProbability;
                playground.SetState(new Vector(x, y), state);
            }
        }
    }

    public static void PrepareFreestyle(SimplePlayGround playground)
    {
        AddBlinker(playground, new Vector(4, 4));
        AddBlinker(playground, new Vector(8, 4));
        AddGlider(playground, new Vector(4, 8));
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

    public static void AddSingleCell(SimplePlayGround playground, Vector position)
    {
        playground.SetState(new Vector(position.X, position.Y), true);
    }
    
    // **Muster 1: Blinker (kleiner Oszillator)**
    public static void AddBlinker(SimplePlayGround playground, Vector startPosition)
    { 
        playground.SetState(new Vector(startPosition.X, startPosition.Y), true);
        playground.SetState(new Vector(startPosition.X + 1, startPosition.Y), true);
        playground.SetState(new Vector(startPosition.X + 2, startPosition.Y), true);
    }

    // **Muster 2: Glider (bewegliches Muster)**
    public static void AddGlider(SimplePlayGround playground, Vector startPosition)
    {
        playground.SetState(new Vector(startPosition.X + 2, startPosition.Y), true);
        playground.SetState(new Vector(startPosition.X, startPosition.Y + 1), true);
        playground.SetState(new Vector(startPosition.X + 2, startPosition.Y + 1), true);
        playground.SetState(new Vector(startPosition.X + 1, startPosition.Y + 2), true);
        playground.SetState(new Vector(startPosition.X + 2, startPosition.Y + 2), true);
    }

    // **Muster 3: Toad (größerer Oszillator)**
    public static void AddToad(SimplePlayGround playground, Vector startPosition)
    {
        playground.SetState(new Vector(startPosition.X + 1, startPosition.Y), true);
        playground.SetState(new Vector(startPosition.X + 2, startPosition.Y), true);
        playground.SetState(new Vector(startPosition.X + 3, startPosition.Y), true);
        playground.SetState(new Vector(startPosition.X, startPosition.Y + 1), true);
        playground.SetState(new Vector(startPosition.X + 1, startPosition.Y + 1), true);
        playground.SetState(new Vector(startPosition.X + 2, startPosition.Y + 1), true);
    }

    // **Muster 4: Block (stabiler Zustand)**
    public static void AddBlock(SimplePlayGround playground, Vector startPosition)
    {
        playground.SetState(new Vector(startPosition.X, startPosition.Y), true);
        playground.SetState(new Vector(startPosition.X + 1, startPosition.Y), true);
        playground.SetState(new Vector(startPosition.X, startPosition.Y + 1), true);
        playground.SetState(new Vector(startPosition.X + 1, startPosition.Y + 1), true);
    }

    // **Muster 5: Beacon (kleiner oszillierender Zustand)**
    public static void AddBeacon(SimplePlayGround playground, Vector startPosition)
    {
        // Oberer linker Block
        playground.SetState(new Vector(startPosition.X, startPosition.Y), true);
        playground.SetState(new Vector(startPosition.X + 1, startPosition.Y), true);
        playground.SetState(new Vector(startPosition.X, startPosition.Y + 1), true);
        playground.SetState(new Vector(startPosition.X + 1, startPosition.Y + 1), true);

        // Unterer rechter Block
        playground.SetState(new Vector(startPosition.X + 2, startPosition.Y + 2), true);
        playground.SetState(new Vector(startPosition.X + 3, startPosition.Y + 2), true);
        playground.SetState(new Vector(startPosition.X + 2, startPosition.Y + 3), true);
        playground.SetState(new Vector(startPosition.X + 3, startPosition.Y + 3), true);
    }
}