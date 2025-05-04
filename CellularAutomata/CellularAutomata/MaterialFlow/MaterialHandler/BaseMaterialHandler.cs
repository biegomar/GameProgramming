using System.Runtime.CompilerServices;
using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow.MaterialHandler;

public abstract class BaseMaterialHandler(Vector dimension, uint seed = 100)
{
    /// <summary>
    /// A constant dictionary that maps each byte key (0-7) to a relative neighbor vector.
    /// </summary>
    protected static readonly IReadOnlyDictionary<uint, Vector> NeighborVectors = new Dictionary<uint, Vector>
    {
        { 0, new Vector(0, -1) },  // Oben
        { 1, new Vector(1, -1) },  // Oben rechts
        { 2, new Vector(1, 0) },   // Rechts
        { 3, new Vector(1, 1) },   // Unten rechts
        { 4, new Vector(0, 1) },   // Unten
        { 5, new Vector(-1, 1) },  // Unten links
        { 6, new Vector(-1, 0) },  // Links
        { 7, new Vector(-1, -1) }  // Oben links
    };
    
    protected readonly PseudoRandom pseudoRandom = new (seed);
    
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
        return new MaterialMovement(new Material(position, cell.WithCounter(Math.Min(15, cell.GetCounter() + 1))), null);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static MaterialMovement? SetNewMaterialPositions(Vector fromPosition, Cell cellForSource, Vector toPosition, Cell cellForDestination)
    {
        return new MaterialMovement(new Material(fromPosition, cellForSource), new Material(toPosition, cellForDestination));
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
    protected bool IsWater(CellType cellType)
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