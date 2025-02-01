using CellularAutomata;

namespace SkiasGameOfLive;

public static class GameOfLifeInitializer
{
    public static void Randomize(PlayGround<bool> playground, double aliveProbability = 0.2)
    {
        var random = new Random();

        for (int x = 0; x < playground.Dimension.X; x++)
        {
            for (int y = 0; y < playground.Dimension.Y; y++)
            {
                playground[new Vector(x, y, 0)] = random.NextDouble() < aliveProbability;
            }
        }
    }

    public static void AddSingleCell(PlayGround<bool> playground, int x, int y)
    {
        playground[new Vector(x, y, 0)] = true;
    }

    public static void AddCheckerboard(PlayGround<bool> playground)
    {
        for (int y = 0; y < playground.Dimension.Y; y++)
        {
            AddCheckerLine(playground, y);
        }
    }
    
    private static void AddCheckerLine(PlayGround<bool> playground, int row)
    {
        for (int x = 0; x < playground.Dimension.X; x++)
        {
            playground[new Vector(x, row, 0)] = int.IsEvenInteger(x) && int.IsEvenInteger(row) || int.IsOddInteger(x) && int.IsOddInteger(row);
        } 
    }

    public static void AddSingleLineWithCellOnEveryXColumn(PlayGround<bool> playground, int distance, int row)
    {
        for (int x = 0; x < playground.Dimension.X; x++)
        {
            playground[new Vector(x, row, 0)] = x % distance == 0;
        }
    }

    public static void AddSingleColumnWithCellOnEveryYRow(PlayGround<bool> playground, int distance, int column)
    {
        for (int y = 0; y < playground.Dimension.Y; y++)
        {
            playground[new Vector(column, y, 0)] = y % distance == 0;
        }
    }

    public static void AddSingleCell(PlayGround<bool> playground, Vector position)
    {
        playground[position] = true;
    }
    
    // **Muster 1: Blinker (kleiner Oszillator)**
    public static void AddBlinker(PlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X + 2, startPosition.Y, 0)] = true;
    }

    // **Muster 2: Glider (bewegliches Muster)**
    public static void AddGlider(PlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X + 2, startPosition.Y, 0)] = true;        // Zelle oben rechts
        playground[new Vector(startPosition.X, startPosition.Y + 1, 0)] = true;        // Zelle Mitte links
        playground[new Vector(startPosition.X + 2, startPosition.Y + 1, 0)] = true;    // Zelle Mitte rechts
        playground[new Vector(startPosition.X + 1, startPosition.Y + 2, 0)] = true;    // Zelle Mitte unten
        playground[new Vector(startPosition.X + 2, startPosition.Y + 2, 0)] = true;    // Zelle unten rechts 
    }

    // **Muster 3: Toad (größerer Oszillator)**
    public static void AddToad(PlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X + 1, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X + 2, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X + 3, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X, startPosition.Y + 1, 0)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1, 0)] = true;
        playground[new Vector(startPosition.X + 2, startPosition.Y + 1, 0)] = true;
    }

    // **Muster 4: Block (stabiler Zustand)**
    public static void AddBlock(PlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X, startPosition.Y + 1, 0)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1, 0)] = true;
    }

    // **Muster 5: Beacon (kleiner oszillierender Zustand)**
    public static void AddBeacon(PlayGround<bool> playground, Vector startPosition)
    {
        // Oberer linker Block
        playground[new Vector(startPosition.X, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X, startPosition.Y + 1, 0)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1, 0)] = true;

        // Unterer rechter Block
        playground[new Vector(startPosition.X + 2, startPosition.Y + 2, 0)] = true;
        playground[new Vector(startPosition.X + 3, startPosition.Y + 2, 0)] = true;
        playground[new Vector(startPosition.X + 2, startPosition.Y + 3, 0)] = true;
        playground[new Vector(startPosition.X + 3, startPosition.Y + 3, 0)] = true;
    }
}