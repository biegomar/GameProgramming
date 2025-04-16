using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CellularAutomata.Cells;
using CellularAutomata.Interfaces;

namespace CellularAutomata.MaterialFlow;

public sealed class SandRuleSet(Vector dimension)
{
    private const CellBrightness Solid = CellBrightness.Solid;
    private const CellBrightness Empty = CellBrightness.Empty;
    private readonly Random random = new ();

    public CellBrightness ApplyRules(IPlayGround playGround, Vector position)
    {
        // var cellState = playGround[position];
        //
        // var pushCellNeighbors = GetPushCellNeighboursState(playGround, position);
        //
        // // First look at a cell with state - so we push the grain.
        //
        // if (IsSand(cellState))
        // {
        //     if (pushCellNeighbors.Bottom == Empty)
        //     {
        //         return Empty;
        //     }
        //
        //     if (pushCellNeighbors is { BottomRight: Empty, Right: Empty } && position.Y < playGround.Dimension.Y - 1)
        //     {
        //         if (pushCellNeighbors is { BottomLeft: Empty, Left: Empty } && !playGround.IsProcessedRight(new Vector(position.X - 2, position.Y)) && position.Y < playGround.Dimension.Y - 1)
        //         {
        //             if (WillMoveRight())
        //             {
        //                 playGround.MarkAsProcessedRight(position); 
        //             }
        //             else
        //             {
        //                 playGround.MarkAsProcessedLeft(position);
        //             }
        //             return Empty;
        //         }
        //         
        //         playGround.MarkAsProcessedRight(position);
        //         return Empty;
        //     }
        //     
        //     if (pushCellNeighbors is { BottomLeft: Empty, Left: Empty } && !playGround.IsProcessedRight(new Vector(position.X - 2, position.Y)) && position.Y < playGround.Dimension.Y - 1)
        //     {
        //         playGround.MarkAsProcessedLeft(position);
        //         return Empty;
        //     }
        //     
        //     return cellState;
        // }
        //
        // if (IsSolid(cellState))
        // {
        //     return Solid;
        // }
        //
        // // We are sure. That cell is empty. Now we pull the grain.
        //
        // var topRowNeighbors = GetTopRowNeighborsState(playGround, position);
        //
        // // Prio 1: grain above me
        // if (IsSand(topRowNeighbors.Top))
        // {
        //     return topRowNeighbors.Top;
        // }
        //
        // if (playGround.IsProcessedRight(new Vector(position.X - 1, position.Y - 1)))
        // {
        //     return topRowNeighbors.TopLeft;
        // }
        //
        // if (playGround.IsProcessedLeft(new Vector(position.X + 1, position.Y - 1)))
        // {
        //     return topRowNeighbors.TopRight;
        // }
        //
        // return Empty;
        return Empty;
    }

    public MaterialMovement? ApplyMaterialRules(IPlayGround playGround, Vector position)
    {
        var cell = playGround.GetCell(position);

        if (cell.Type == CellType.Empty) return null;

        if (cell.Type == CellType.Sand)
        {
            var bottomCell = GetCell(playGround, new Vector(position.X, position.Y + 1));
            if (bottomCell.Type == CellType.Empty && position.Y < playGround.Dimension.Y - 1)
            {
                return new MaterialMovement(new Material(position, bottomCell with {}), new Material(new Vector(position.X, position.Y + 1), cell with {}));
            }
            
            var rightCell = GetCell(playGround, new Vector(position.X + 1, position.Y));
            var rightBottomCell = GetCell(playGround, new Vector(position.X + 1, position.Y + 1));

            if (rightCell.Type == CellType.Empty && rightBottomCell.Type == CellType.Empty)
            {
                return new MaterialMovement(new Material(position, new Cell(CellType.Empty, CellBrightness.Empty)), new Material(new Vector(position.X + 1, position.Y + 1), cell with { }));
            }
            
            var leftCell = GetCell(playGround, new Vector(position.X - 1, position.Y));
            var leftBottomCell = GetCell(playGround, new Vector(position.X - 1, position.Y + 1));
            var leftOpponentCell = GetCell(playGround, new Vector(position.X - 2, position.Y));

            if (leftCell.Type == CellType.Empty && leftBottomCell.Type == CellType.Empty && IsSolidOrEmpty(leftOpponentCell.Type))
            {
                return new MaterialMovement(new Material(position, new Cell(CellType.Empty, CellBrightness.Empty)), new Material(new Vector(position.X - 1, position.Y + 1), cell with { }));
            }
        }

        return new MaterialMovement(new Material(position, cell with {}), null);
    }

    public IPlayGround ApplySpawnRules(IPlayGround playGround, Vector spawnPosition, Vector brushSize, double probability)
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
                if (cell.Type == CellType.Empty)
                {
                    if (random.NextDouble() < probability)
                    {
                        playGround.SetCell(new Vector(x, y), new Cell(CellType.Sand, GetRandomSandCellState()));   
                    }
                }
            }    
        }
        
        return playGround;
    }

    private Cell GetCell(IPlayGround playGround, Vector position)
    {
        return IsWithinBounds(position) ? playGround.GetCell(position) : new Cell(CellType.Solid, CellBrightness.Solid);
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
    private bool IsSandOrSolid(CellType cellType)
    {
        return (byte)cellType == 1 || (byte)cellType == 2;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CellBrightness GetRandomSandCellState()
    {
        var randomValue = random.Next(2, 6);

        return (CellBrightness)randomValue;

    }
}