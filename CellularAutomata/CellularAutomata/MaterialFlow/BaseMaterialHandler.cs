using System.Runtime.CompilerServices;
using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow;

public abstract class BaseMaterialHandler(Vector dimension, uint seed = 100)
{
    private readonly PseudoRandom pseudoRandom = new (seed);
    
    protected const CellType Solid = CellType.Solid;
    protected const CellType Empty = CellType.Empty;
    protected const CellType Sand = CellType.Sand;

    public abstract MaterialMovement? ApplyRules(PlayGround playGround, Vector position, Cell cell);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Cell GetCell(PlayGround playGround, Vector position)
    {
        return IsWithinBounds(position) ? playGround.GetCell(position) : new Cell(Solid, CellColor.Solid);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsWithinBounds(Vector position )
    {
        return (uint)position.X < (uint)dimension.X && (uint)position.Y < (uint)dimension.Y;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected MaterialMovement DontMove(Vector position, Cell cell)
    {
        var immobileCell = cell;
        if (!cell.IsFlagSet(1))
        {
            immobileCell = immobileCell.WithFlag(1, true);
        }
        else if (!cell.IsFlagSet(2))
        {
            immobileCell = immobileCell.WithFlag(2, true);
        }
        
        return new MaterialMovement(new Material(position, immobileCell.WithFlag(0, true)), null);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static MaterialMovement? SetNewMaterialPositions(Vector fromPosition, Cell cellForSource, Vector toPosition, Cell cellForDestination)
    {
        return new MaterialMovement(new Material(fromPosition, cellForSource.WithFlag(0, true)), new Material(toPosition, cellForDestination.WithFlag(0, true)));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool IsSolidOrEmpty(CellType cellType)
    {
        return (byte)cellType <= 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool IsEmpty(CellType cellType)
    {
        return (byte)cellType == 0;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool IsSolid(CellType cellType)
    {
        return (byte)cellType == 1;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool IsLiquidOrEmpty(CellType cellType)
    {
        return (byte)cellType == 0 || (byte)cellType == 3;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool IsLiquidOrSolid(CellType cellType)
    {
        return (byte)cellType == 1 || (byte)cellType == 3;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool IsSolidOrLiquidOrEmpty(CellType cellType)
    {
        return (byte)cellType <= 1 || (byte)cellType == 3;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool IsLiquid(CellType cellType)
    {
        return (byte)cellType == 3;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool IsNonSlidingOrEmpty(CellType cellType)
    {
        return (byte)cellType <= 2;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool WillMoveRight()
    {
        return pseudoRandom.Chance(50);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool WillMoveAtAll(int probability)
    {
        return pseudoRandom.Chance(probability);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected uint GetIndex(Vector position)
    {
        return (uint)position.Y * (uint)dimension.X + (uint)position.X;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool IsOccupied(Vector position, bool[] occupiedCells)
    {
        return occupiedCells[GetIndex(position)];
    }
    
    private void AddToOccupiedCells(Vector position, bool[] occupiedCells)
    {
        if (occupiedCells[GetIndex(position)])
            return;
        
        occupiedCells[GetIndex(position)] = true;
    }
}