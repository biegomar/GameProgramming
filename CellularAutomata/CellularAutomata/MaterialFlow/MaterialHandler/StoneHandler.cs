using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow.MaterialHandler;

public class StoneHandler(Vector dimension, uint seed = 100) : BaseMaterialHandler(dimension, seed)
{
    public override MaterialMovement? ApplyRules(PlayGround playGround, Vector position, Cell cell)
    {
        var bottomCell = GetCell(playGround, new Vector(position.X, position.Y + 1));
        
        // direct way: bottom cell is free
        if (IsEmpty(bottomCell.Type))
        {
            return SetNewMaterialPositions(position, bottomCell, new Vector(position.X, position.Y + 1), cell);
        }
        
        // Last option: sink into liquid
        var isBottomFreeToSink = IsLiquid(bottomCell.Type) && bottomCell.GetCounter() >= 2 && cell.GetCounter() >= 2;
        if (isBottomFreeToSink)
        {
            bottomCell = bottomCell.WithCounter(0);
            cell = cell.WithCounter(0);
            return SetNewMaterialPositions(position, bottomCell, new Vector(position.X, position.Y + 1), cell);
        }
        
        return DontMove(position, cell);
    }
}