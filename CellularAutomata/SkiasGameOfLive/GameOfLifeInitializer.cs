using CellularAutomata;

namespace SkiasGameOfLive;

public static class GameOfLifeInitializer
{
    public static void Randomize(IPlayGround<bool> playground, double aliveProbability = 0.2)
    {
        var random = new Random();

        playground.ForEachCell(position =>
        {
            bool state = random.NextDouble() < aliveProbability;
            playground[position] = state;
        });
    }

    public static void AddSingleCell(IPlayGround<bool> playground, int x, int y)
    {
        playground[(x, y, 0)] = true;
    }

    public static void AddCheckerboard(IPlayGround<bool> playground)
    {
        for (int y = 0; y < playground.Dimension.Y; y++)
        {
            AddCheckerLine(playground, y);
        }
    }
    
    private static void AddCheckerLine(IPlayGround<bool> playground, int row)
    {
        for (int x = 0; x < playground.Dimension.X; x++)
        {
            playground[(x, row, 0)] = int.IsEvenInteger(x) && int.IsEvenInteger(row) || int.IsOddInteger(x) && int.IsOddInteger(row);
        } 
    }

    public static void AddSingleLineWithCellOnEveryXColumn(IPlayGround<bool> playground, int distance, int row)
    {
        for (int x = 0; x < playground.Dimension.X; x++)
        {
            playground[(x, row, 0)] = x % distance == 0;
        } 
    }

    public static void AddSingleColumnWithCellOnEveryYRow(IPlayGround<bool> playground, int distance, int column)
    {
        for (int y = 0; y < playground.Dimension.Y; y++)
        {
            playground[(column, y, 0)] = y % distance == 0;
        } 
    }

    public static void AddSingleCell(IPlayGround<bool> playground, Vector position)
    {
        playground[position] = true;
    }
    
    public static void AddSandCellStateToCell(IPlayGround<SandCellState> playground, Vector position, SandCellState state)
    {
        playground[position] = state; 
    }
    
    // **Muster 1: Blinker (kleiner Oszillator)**
    public static void AddBlinker(IPlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X + 2, startPosition.Y, 0)] = true;
    }

    // **Muster 2: Glider (bewegliches Muster)**
    public static void AddGlider(IPlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X + 2, startPosition.Y, 0)] = true;        // Zelle oben rechts
        playground[new Vector(startPosition.X, startPosition.Y + 1, 0)] = true;        // Zelle Mitte links
        playground[new Vector(startPosition.X + 2, startPosition.Y + 1, 0)] = true;    // Zelle Mitte rechts
        playground[new Vector(startPosition.X + 1, startPosition.Y + 2, 0)] = true;    // Zelle Mitte unten
        playground[new Vector(startPosition.X + 2, startPosition.Y + 2, 0)] = true;    // Zelle unten rechts 
    }

    // **Muster 3: Toad (größerer Oszillator)**
    public static void AddToad(IPlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X + 1, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X + 2, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X + 3, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X, startPosition.Y + 1, 0)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1, 0)] = true;
        playground[new Vector(startPosition.X + 2, startPosition.Y + 1, 0)] = true; 
    }

    // **Muster 4: Block (stabiler Zustand)**
    public static void AddBlock(IPlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y, 0)] = true;
        playground[new Vector(startPosition.X, startPosition.Y + 1, 0)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1, 0)] = true; 
    }

    // **Muster 5: Beacon (kleiner oszillierender Zustand)**
    public static void AddBeacon(IPlayGround<bool> playground, Vector startPosition)
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
    
    public static void GenerateSandHourglass(IPlayGround<SandCellState> playground)
    {
        var dimension = playground.Dimension;
        var width = (int)dimension.X;
        var height = (int)dimension.Y;

        int midX = width / 2; // Mitte der Breite
        int midY = height / 2; // Mitte der Höhe

        // Sanduhr von oben nach unten aufbauen
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (IsOutline(x, y, width, height) && !IsConnection(x, y, midX, midY))
                {
                    playground[new Vector(x, y, 0)] = SandCellState.Solid;
                }
                else if (IsTopSand(x, y, midX, midY))
                {
                    playground[new Vector(x, y, 0)] = SandCellState.Sand;
                }
                else if (IsConnection(x, y, midX, midY))
                {
                    playground[new Vector(x, y, 0)] = SandCellState.Sand;
                }
                else if (IsBottomEmpty(x, y, midX, midY))
                {
                    playground[new Vector(x, y, 0)] = SandCellState.Empty;
                }
            }
        }
    }

    // Überprüft, ob die Zelle Teil des äußeren Rahmens ist
    private static bool IsOutline(int x, int y, int width, int height)
    {
        int midX = width / 2;
        int midY = height / 2;

        // Obere Hälfte (Sanduhr-Umrandung für oberen Kolben)
        if (y <= midY && Math.Abs(x - midX) == (midY - y))
        {
            return true; // Rand für Top (triangular)
        }

        // Untere Hälfte (Sanduhr-Umrandung für unteren Kolben)
        if (y >= midY && Math.Abs(x - midX) == (y - midY))
        {
            return true; // Rand für Bottom (inverted triangular)
        }

        // Kein Rand
        return false;
    }


    // Überprüft, ob die Zelle zum oberen (gefüllten) Sand passt
    private static bool IsTopSand(int x, int y, int midX, int midY)
    {
        return y < midY && Math.Abs(x - midX) <= (midY - y - 1);
    }

    // Überprüft, ob die Zelle die Verbindung (einen Punkt breit) zwischen den Kolben ist
    private static bool IsConnection(int x, int y, int midX, int midY)
    {
        // Genau die Mitte der Sanduhr (1 Zelle)
        return y == midY && x == midX;

    }

    // Überprüft, ob die Zelle im unteren Kolben (leer) liegt
    private static bool IsBottomEmpty(int x, int y, int midX, int midY)
    {
        return y > midY && Math.Abs(x - midX) <= (y - midY - 1);
    }

}