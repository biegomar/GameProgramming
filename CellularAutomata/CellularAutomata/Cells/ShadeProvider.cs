namespace CellularAutomata.Cells;

public static class ShadeProvider
{
    private static readonly Random random = new();
    
    public static CellColor GenerateRandomColor(CellType type)
    {
        return type switch
        {
            CellType.Sand => (CellColor)random.Next(2, 12),
            _ => CellColor.Empty
        };
    }
}