using CellularAutomata;
using CellularAutomata.Cells;

namespace GameOfLive;

public class GameOfLifeInitializer
{
    public static void Randomize(PlayGround playground, double aliveProbability = 0.2)
    {
        var random = new Random();

        for (int x = 0; x < playground.Dimension.X; x++)
        {
            for (int y = 0; y < playground.Dimension.Y; y++)
            {
                playground[new Vector(x, y)] = random.NextDouble() < aliveProbability ? CellBrightness.Solid : CellBrightness.Empty;
            }
        }
    }
    
    // **Muster 1: Blinker (kleiner Oszillator)**
    public static void AddBlinker(PlayGround playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = CellBrightness.Solid;
    }

    // **Muster 2: Glider (bewegliches Muster)**
    public static void AddGlider(PlayGround playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = CellBrightness.Solid;        // Zelle oben rechts
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = CellBrightness.Solid;        // Zelle Mitte links
        playground[new Vector(startPosition.X + 2, startPosition.Y + 1)] = CellBrightness.Solid;    // Zelle Mitte rechts
        playground[new Vector(startPosition.X + 1, startPosition.Y + 2)] = CellBrightness.Solid;    // Zelle Mitte unten
        playground[new Vector(startPosition.X + 2, startPosition.Y + 2)] = CellBrightness.Solid;    // Zelle unten rechts 
    }

    // **Muster 3: Toad (größerer Oszillator)**
    public static void AddToad(PlayGround playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 3, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 2, startPosition.Y + 1)] = CellBrightness.Solid;
    }

    // **Muster 4: Block (stabiler Zustand)**
    public static void AddBlock(PlayGround playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1)] = CellBrightness.Solid;
    }

    // **Muster 5: Beacon (kleiner oszillierender Zustand)**
    public static void AddBeacon(PlayGround playground, Vector startPosition)
    {
        // Oberer linker Block
        playground[new Vector(startPosition.X, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1)] = CellBrightness.Solid;

        // Unterer rechter Block
        playground[new Vector(startPosition.X + 2, startPosition.Y + 2)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 3, startPosition.Y + 2)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 2, startPosition.Y + 3)] = CellBrightness.Solid;
        playground[new Vector(startPosition.X + 3, startPosition.Y + 3)] = CellBrightness.Solid;
    }
}