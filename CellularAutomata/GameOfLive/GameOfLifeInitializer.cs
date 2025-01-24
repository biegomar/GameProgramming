using System;
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
                // Mit einer Wahrscheinlichkeit von 20% erhält die Zelle den Zustand "lebendig"
                playground[new Vector(x, y, 0)] = random.NextDouble() < aliveProbability;
            }
        }
    }
}