namespace CellularAutomata.Cells;

public static class ShadeProvider
{
    private static readonly Random random = new();
    
    public static CellBrightness GenerateRandomBrightness(CellType type)
    {
        return type switch
        {
            CellType.Sand => (CellBrightness)random.Next(2, 12),
            _ => CellBrightness.Empty
        };
    }
}