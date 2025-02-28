using CellularAutomata;

public class GameOfLifeInitializer
{
    public static void Randomize(PlayGround<bool> playground, double aliveProbability = 0.2)
    {
        var random = new Random();

        for (int x = 0; x < playground.Dimension.X; x++)
        {
            for (int y = 0; y < playground.Dimension.Y; y++)
            {
                playground[new Vector(x, y)] = random.NextDouble() < aliveProbability;
            }
        }
    }
    
    // **Muster 1: Blinker (kleiner Oszillator)**
    public static void AddBlinker(PlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X, startPosition.Y)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = true;
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = true;
    }

    // **Muster 2: Glider (bewegliches Muster)**
    public static void AddGlider(PlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = true;        // Zelle oben rechts
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = true;        // Zelle Mitte links
        playground[new Vector(startPosition.X + 2, startPosition.Y + 1)] = true;    // Zelle Mitte rechts
        playground[new Vector(startPosition.X + 1, startPosition.Y + 2)] = true;    // Zelle Mitte unten
        playground[new Vector(startPosition.X + 2, startPosition.Y + 2)] = true;    // Zelle unten rechts 
    }

    // **Muster 3: Toad (größerer Oszillator)**
    public static void AddToad(PlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = true;
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = true;
        playground[new Vector(startPosition.X + 3, startPosition.Y)] = true;
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1)] = true;
        playground[new Vector(startPosition.X + 2, startPosition.Y + 1)] = true;
    }

    // **Muster 4: Block (stabiler Zustand)**
    public static void AddBlock(PlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X, startPosition.Y)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = true;
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1)] = true;
    }

    // **Muster 5: Beacon (kleiner oszillierender Zustand)**
    public static void AddBeacon(PlayGround<bool> playground, Vector startPosition)
    {
        // Oberer linker Block
        playground[new Vector(startPosition.X, startPosition.Y)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = true;
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1)] = true;

        // Unterer rechter Block
        playground[new Vector(startPosition.X + 2, startPosition.Y + 2)] = true;
        playground[new Vector(startPosition.X + 3, startPosition.Y + 2)] = true;
        playground[new Vector(startPosition.X + 2, startPosition.Y + 3)] = true;
        playground[new Vector(startPosition.X + 3, startPosition.Y + 3)] = true;
    }
}