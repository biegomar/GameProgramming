using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow.MaterialHandler;

public class IceHandler(Vector dimension, uint seed = 100) : BaseMaterialHandler(dimension, seed)
{
    public override MaterialMovement? ApplyRules(PlayGround playGround, Vector position, Cell cell)
    {
        var randomVector = NeighborVectors[pseudoRandom.Next(7)];
        var positionToCheck = new Vector(position.X + randomVector.X, position.Y + randomVector.Y);
        
        var randomNeighbor = GetCell(playGround, positionToCheck);

        if (IsWater(randomNeighbor.Type) && pseudoRandom.Chance(7) && randomNeighbor.GetCounter() >= 2 && cell.GetCounter() >= 2)
        {
            return SetNewMaterialPositions(position, cell, positionToCheck, new Cell(CellType.Ice, ShadeProvider.GenerateColor(CellType.Ice)));
        }
        
        return DontMove(position, cell);
    }
}