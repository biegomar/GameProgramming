using System.Runtime.CompilerServices;
using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow;

public sealed class MaterialRuleSet(Vector dimension, uint seed = 100)
{
    private const CellType Solid = CellType.Solid;
    private const CellType Empty = CellType.Empty;
    private const CellType Sand = CellType.Sand;
    private readonly Random random = new ();
    private readonly PseudoRandom pseudoRandom = new (seed);

    public MaterialMovement? ApplyRules(PlayGround playGround, Vector position)
    {
        var cell = playGround.GetCell(position);

        if (IsEmpty(cell.Type)) return null;

        if (IsSand(cell.Type))
        {
            return HandleSand(playGround, position, cell);
        }

        // dont move
        return DontMove(position, cell);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private MaterialMovement HandleSand(PlayGround playGround, Vector position, Cell cell)
    {
        // direct way: bottom cell is free
        var bottomCell = GetCell(playGround, new Vector(position.X, position.Y + 1));
        if (bottomCell.Type == Empty)
        {
            return new MaterialMovement(new Material(position, bottomCell with {}), new Material(new Vector(position.X, position.Y + 1), cell with {}));
        }
            
        var rightCell = GetCell(playGround, new Vector(position.X + 1, position.Y));
        var rightBottomCell = GetCell(playGround, new Vector(position.X + 1, position.Y + 1));
        var leftCell = GetCell(playGround, new Vector(position.X - 1, position.Y));
        var leftBottomCell = GetCell(playGround, new Vector(position.X - 1, position.Y + 1));
        var leftOpponentCell = GetCell(playGround, new Vector(position.X - 2, position.Y));

        // right way is free
        if (rightCell.Type == Empty && rightBottomCell.Type == Empty)
        {
            // the left way as well
            if (leftCell.Type == Empty && leftBottomCell.Type == Empty && (IsSolidOrEmpty(leftOpponentCell.Type) || playGround.IsMarkedCell(new Vector(position.X - 2, position.Y))))
            {
                // move by 80%
                if (WillMoveAtAll(50))
                {
                    // pseudo-random choice
                    if (WillMoveRight())
                    {
                        // go right
                        return new MaterialMovement(new Material(position, new Cell(Empty, CellColor.Empty)), new Material(new Vector(position.X + 1, position.Y + 1), cell with { }));
                    }
                    
                    // go left
                    playGround.MarkCell(position);
                    return new MaterialMovement(new Material(position, new Cell(Empty, CellColor.Empty)), new Material(new Vector(position.X - 1, position.Y + 1), cell with { }));    
                }
                    
                // dont move
                return DontMove(position, cell);
            }

            // go right by 90%
            if (WillMoveAtAll(50))
            {
                return new MaterialMovement(new Material(position, new Cell(Empty, CellColor.Empty)), new Material(new Vector(position.X + 1, position.Y + 1), cell with { }));
            }
                
            // dont move
            return DontMove(position, cell);
        }

        if (leftCell.Type == Empty && leftBottomCell.Type == Empty && (IsSolidOrEmpty(leftOpponentCell.Type) || playGround.IsMarkedCell(new Vector(position.X - 2, position.Y))))
        {
            // go left
            playGround.MarkCell(position);
            return new MaterialMovement(new Material(position, new Cell(Empty, CellColor.Empty)), new Material(new Vector(position.X - 1, position.Y + 1), cell with { }));
        }

        return DontMove(position, cell);
    }

    public PlayGround ApplySpawnRules(PlayGround playGround, Vector spawnPosition, Vector brushSize, double probability)
    {
        var startX = spawnPosition.X;
        var endX = spawnPosition.X + brushSize.X - 1;
        
        var startY = spawnPosition.Y;
        var endY = spawnPosition.Y + brushSize.Y - 1;

        for (var x = startX; x <= endX; x++)
        {
            for (var y = startY; y <= endY; y++)
            {
                var cell = GetCell(playGround, new Vector(x, y));
                if (cell.Type == Empty)
                {
                    if (random.NextDouble() < probability)
                    {
                        playGround.SetCell(new Vector(x, y), new Cell(Sand, ShadeProvider.GenerateRandomColor(CellType.Sand)));   
                    }
                }
            }    
        }
        
        return playGround;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Cell GetCell(PlayGround playGround, Vector position)
    {
        return IsWithinBounds(position) ? playGround.GetCell(position) : new Cell(Solid, CellColor.Solid);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private MaterialMovement DontMove(Vector position, Cell cell)
    {
        return new MaterialMovement(new Material(position, cell with {}), null);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsWithinBounds(Vector position )
    {
        return (uint)position.X < (uint)dimension.X && (uint)position.Y < (uint)dimension.Y;
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
    private bool IsSolidOrEmpty(CellType cellType)
    {
        return (byte)cellType <= 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool WillMoveRight()
    {
        return pseudoRandom.Chance(50);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool WillMoveAtAll(int probability)
    {
        return pseudoRandom.Chance(probability);
    }

}