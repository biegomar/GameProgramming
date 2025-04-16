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
                var state = random.NextDouble() < aliveProbability;
                playground.SetCell(new Vector(x, y), state 
                    ? new Cell(CellType.Solid, CellBrightness.Solid)
                    : new Cell(CellType.Empty, CellBrightness.Empty));
            }
        }
    }
    
    // **Muster 1: Blinker (kleiner Oszillator)**
    public static void AddBlinker(PlayGround playground, Vector startPosition)
    { 
        playground.SetCell(new Vector(startPosition.X, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
    }

    // **Muster 2: Glider (bewegliches Muster)**
    public static void AddGlider(PlayGround playground, Vector startPosition)
    {
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y + 2), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y + 2), new Cell(CellType.Solid, CellBrightness.Solid));
    }

    // **Muster 3: Toad (größerer Oszillator)**
    public static void AddToad(PlayGround playground, Vector startPosition)
    {
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 3, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
    }

    // **Muster 4: Block (stabiler Zustand)**
    public static void AddBlock(PlayGround playground, Vector startPosition)
    {
        playground.SetCell(new Vector(startPosition.X, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
    }

    // **Muster 5: Beacon (kleiner oszillierender Zustand)**
    public static void AddBeacon(PlayGround playground, Vector startPosition)
    {
        // Oberer linker Block
        playground.SetCell(new Vector(startPosition.X, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));

        // Unterer rechter Block
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y + 2), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 3, startPosition.Y + 2), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y + 3), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 3, startPosition.Y + 3), new Cell(CellType.Solid, CellBrightness.Solid));
    }
}