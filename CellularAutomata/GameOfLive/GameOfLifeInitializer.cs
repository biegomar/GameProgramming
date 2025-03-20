using CellularAutomata;

public class GameOfLifeInitializer
{
    public static void Randomize(PlayGround playground, double aliveProbability = 0.2)
    {
        var random = new Random();

        for (int x = 0; x < playground.Dimension.X; x++)
        {
            for (int y = 0; y < playground.Dimension.Y; y++)
            {
                playground[new Vector(x, y)] = random.NextDouble() < aliveProbability ? CellState.Solid : CellState.Empty;
            }
        }
    }
    
    // **Muster 1: Blinker (kleiner Oszillator)**
    public static void AddBlinker(PlayGround playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X, startPosition.Y)] = CellState.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = CellState.Solid;
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = CellState.Solid;
    }

    // **Muster 2: Glider (bewegliches Muster)**
    public static void AddGlider(PlayGround playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = CellState.Solid;        // Zelle oben rechts
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = CellState.Solid;        // Zelle Mitte links
        playground[new Vector(startPosition.X + 2, startPosition.Y + 1)] = CellState.Solid;    // Zelle Mitte rechts
        playground[new Vector(startPosition.X + 1, startPosition.Y + 2)] = CellState.Solid;    // Zelle Mitte unten
        playground[new Vector(startPosition.X + 2, startPosition.Y + 2)] = CellState.Solid;    // Zelle unten rechts 
    }

    // **Muster 3: Toad (größerer Oszillator)**
    public static void AddToad(PlayGround playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = CellState.Solid;
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = CellState.Solid;
        playground[new Vector(startPosition.X + 3, startPosition.Y)] = CellState.Solid;
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = CellState.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1)] = CellState.Solid;
        playground[new Vector(startPosition.X + 2, startPosition.Y + 1)] = CellState.Solid;
    }

    // **Muster 4: Block (stabiler Zustand)**
    public static void AddBlock(PlayGround playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X, startPosition.Y)] = CellState.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = CellState.Solid;
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = CellState.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1)] = CellState.Solid;
    }

    // **Muster 5: Beacon (kleiner oszillierender Zustand)**
    public static void AddBeacon(PlayGround playground, Vector startPosition)
    {
        // Oberer linker Block
        playground[new Vector(startPosition.X, startPosition.Y)] = CellState.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = CellState.Solid;
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = CellState.Solid;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1)] = CellState.Solid;

        // Unterer rechter Block
        playground[new Vector(startPosition.X + 2, startPosition.Y + 2)] = CellState.Solid;
        playground[new Vector(startPosition.X + 3, startPosition.Y + 2)] = CellState.Solid;
        playground[new Vector(startPosition.X + 2, startPosition.Y + 3)] = CellState.Solid;
        playground[new Vector(startPosition.X + 3, startPosition.Y + 3)] = CellState.Solid;
    }
}