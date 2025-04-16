using CellularAutomata;
using CellularAutomata.Cells;
using CellularAutomata.Interfaces;

namespace GameOfLive;

public class GameOfLifeInitializer
{
    public static void Randomize(IPlayGround playground, double aliveProbability = 0.2)
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
    public static void AddBlinker(IPlayGround playground, Vector startPosition)
    { 
        playground.SetCell(new Vector(startPosition.X, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
    }

    // **Muster 2: Glider (bewegliches Muster)**
    public static void AddGlider(IPlayGround playground, Vector startPosition)
    {
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y + 2), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y + 2), new Cell(CellType.Solid, CellBrightness.Solid));
    }

    // **Muster 3: Toad (größerer Oszillator)**
    public static void AddToad(IPlayGround playground, Vector startPosition)
    {
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 3, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 2, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
    }

    // **Muster 4: Block (stabiler Zustand)**
    public static void AddBlock(IPlayGround playground, Vector startPosition)
    {
        playground.SetCell(new Vector(startPosition.X, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
        playground.SetCell(new Vector(startPosition.X + 1, startPosition.Y + 1), new Cell(CellType.Solid, CellBrightness.Solid));
    }

    // **Muster 5: Beacon (kleiner oszillierender Zustand)**
    public static void AddBeacon(IPlayGround playground, Vector startPosition)
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