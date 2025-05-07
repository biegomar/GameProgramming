using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow.MaterialHandler;

public sealed class IceHandler(Vector dimension, uint seed = 100) : BaseMaterialHandler(dimension, seed)
{
    private int currentIndex = 0;
    
    public override MaterialMovement? ApplyRules(PlayGround playGround, Vector position, Cell cell)
    {
        if (cell.GetCounter() < 2)
        {
            return DontMove(position, cell);    
        }
        
        currentIndex = (currentIndex + 1) % 8;
        var randomVector = NeighborVectors[currentIndex];
        var positionToCheck = new Vector(position.X + randomVector.X, position.Y + randomVector.Y);
        
        var randomNeighbor = GetCell(playGround, positionToCheck);

        if (randomNeighbor.GetCounter() >= 2 && IsWater(randomNeighbor.Type) && pseudoRandom.Chance(7))
        {
            return SetNewMaterialPositions(position, cell, positionToCheck, new Cell(CellType.Ice, ShadeProvider.GenerateColor(CellType.Ice)));
        }
        
        return null;
    }
}