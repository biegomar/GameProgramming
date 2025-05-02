namespace CellularAutomata.Cells;

public static class ShadeProvider
{
    private static readonly Random random = new();
    
    public static CellColor GenerateColor(CellType type)
    {
        return type switch
        {
            CellType.Empty => CellColor.Empty,
            CellType.Solid => CellColor.Solid,
            CellType.Sand => (CellColor)random.Next(2, 12),
            CellType.Water => (CellColor)random.Next(12, 22),
            CellType.Snow => (CellColor)random.Next(22, 32),
            CellType.Ice => (CellColor)random.Next(32, 42),
            CellType.Stone => (CellColor)random.Next(42, 52),
            _ => CellColor.Empty
        };
    }
}