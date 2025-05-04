using System.Runtime.CompilerServices;
using CellularAutomata.Cells;
using CellularAutomata.MaterialFlow.MaterialHandler;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow;

public sealed class MaterialRuleSet(Vector dimension)
{
    private readonly Random random = new ();
    private readonly SandHandler sandHandler = new(dimension);
    private readonly WaterHandler waterHandler = new(dimension);
    private readonly StoneHandler stoneHandler = new(dimension);
    private readonly IceHandler iceHandler = new(dimension);
    private readonly SnowHandler snowHandler = new(dimension);

    public MaterialMovement? ApplyRules(PlayGround playGround, Vector position)
    {
        var cell = playGround.GetCell(position);

        if (IsEmpty(cell.Type)) return null;
        
        if (IsSand(cell.Type))
        {
            return sandHandler.ApplyRules(playGround, position, cell);
        }
        
        if (IsWater(cell.Type))
        {
            return waterHandler.ApplyRules(playGround, position, cell);
        }
        
        if (IsSnow(cell.Type))
        {
            return snowHandler.ApplyRules(playGround, position, cell);
        }
        
        if (IsIce(cell.Type))
        {
            return iceHandler.ApplyRules(playGround, position, cell);
        }
        
        if (IsStone(cell.Type))
        {
            return stoneHandler.ApplyRules(playGround, position, cell);
        }

        // do nothing!
        return null;
    }
    
    public PlayGround ApplySpawnRules(PlayGround playGround, CellType type, Vector spawnPosition, Vector brushSize, double probability)
    {
        var startX = spawnPosition.X;
        var endX = spawnPosition.X + brushSize.X - 1;
        
        var startY = spawnPosition.Y;
        var endY = spawnPosition.Y + brushSize.Y - 1;

        for (var x = startX; x <= endX; x++)
        {
            for (var y = startY; y <= endY; y++)
            {
                var cell = sandHandler.GetCell(playGround, new Vector(x, y));
                if (cell.Type == CellType.Empty || type == CellType.Empty)
                {
                    if (sandHandler.IsWithinBounds(new Vector(x, y)) && random.NextDouble() < probability)
                    {
                        playGround.SetCell(new Vector(x, y), new Cell(type, ShadeProvider.GenerateColor(type)));   
                    }
                }
            }    
        }
        
        return playGround;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsEmpty(CellType cellType)
    {
        return (byte)cellType == 0;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsSand(CellType cellType)
    {
        return (byte)cellType == 2;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWater(CellType cellType)
    {
        return (byte)cellType == 3;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsSnow(CellType cellType)
    {
        return (byte)cellType == 4;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsIce(CellType cellType)
    {
        return (byte)cellType == 5;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsStone(CellType cellType)
    {
        return (byte)cellType == 6;
    }
}