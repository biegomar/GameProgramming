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
            _ => CellColor.Empty
        };
    }
}