using System;
using System.Linq;
using System.Threading.Tasks;
using CellularAutomata;

namespace AvaloniaGameOfLive;

public static class GameOfLifeInitializer
{
    public static void Randomize(IPlayGround<bool> playground, double aliveProbability = 0.2)
    {
        var random = new Random();

        if (playground is PlayGround<bool> playGroundBool)
        {
            Parallel.ForEach(playGroundBool.Cells, cell =>
            {
                var state = random.NextDouble() < aliveProbability;
                playground[cell.Key] = state;
            });
        }
        else if (playground is PlayGroundArray<bool> playGroundArrayBool)
        {
            Parallel.ForEach(playGroundArrayBool.Cells, cell =>
            {
                var state = random.NextDouble() < aliveProbability;
                playGroundArrayBool[(cell.X, cell.Y)] = state;
            });
        }
        
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
            playground[(x, row)] = int.IsEvenInteger(x) && int.IsEvenInteger(row) || int.IsOddInteger(x) && int.IsOddInteger(row);
        } 
    }

    public static void AddSingleLineWithCellOnEveryXColumn(IPlayGround<bool> playground, int distance, int row)
    {
        for (int x = 0; x < playground.Dimension.X; x++)
        {
            playground[(x, row)] = x % distance == 0;
        } 
    }

    public static void AddSingleColumnWithCellOnEveryYRow(IPlayGround<bool> playground, int distance, int column)
    {
        for (int y = 0; y < playground.Dimension.Y; y++)
        {
            playground[(column, y)] = y % distance == 0;
        } 
    }

    public static void AddSingleCell(IPlayGround<bool> playground, Vector position)
    {
        AddSingleCell(playground, position.X, position.Y);
    }
    
    public static void AddSingleCell(IPlayGround<bool> playground, int x, int y)
    {
        playground[(x, y)] = true;
    }
    
    // **Muster 1: Blinker (kleiner Oszillator)**
    public static void AddBlinker(IPlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X, startPosition.Y)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = true;
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = true;
    }

    // **Muster 2: Glider (bewegliches Muster)**
    public static void AddGlider(IPlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = true;        // Zelle oben rechts
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = true;        // Zelle Mitte links
        playground[new Vector(startPosition.X + 2, startPosition.Y + 1)] = true;    // Zelle Mitte rechts
        playground[new Vector(startPosition.X + 1, startPosition.Y + 2)] = true;    // Zelle Mitte unten
        playground[new Vector(startPosition.X + 2, startPosition.Y + 2)] = true;    // Zelle unten rechts 
    }

    // **Muster 3: Toad (größerer Oszillator)**
    public static void AddToad(IPlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = true;
        playground[new Vector(startPosition.X + 2, startPosition.Y)] = true;
        playground[new Vector(startPosition.X + 3, startPosition.Y)] = true;
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1)] = true;
        playground[new Vector(startPosition.X + 2, startPosition.Y + 1)] = true; 
    }

    // **Muster 4: Block (stabiler Zustand)**
    public static void AddBlock(IPlayGround<bool> playground, Vector startPosition)
    {
        playground[new Vector(startPosition.X, startPosition.Y)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y)] = true;
        playground[new Vector(startPosition.X, startPosition.Y + 1)] = true;
        playground[new Vector(startPosition.X + 1, startPosition.Y + 1)] = true; 
    }

    // **Muster 5: Beacon (kleiner oszillierender Zustand)**
    public static void AddBeacon(IPlayGround<bool> playground, Vector startPosition)
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