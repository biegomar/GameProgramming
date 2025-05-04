using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow.MaterialHandler;

public sealed class SnowHandler(Vector dimension, uint seed = 100) : BaseMaterialHandler(dimension, seed)
{
    public override MaterialMovement? ApplyRules(PlayGround playGround, Vector position, Cell cell)
    {
        if (cell.IsFlagSet(0))
        {
            return DontMove(position, cell);
        }
        
        var randomDirection = pseudoRandom.Next(3);
        
        if (randomDirection == 0) // down
        {
            var bottomCell = GetCell(playGround, new Vector(position.X, position.Y + 1));
            var leftCell = GetCell(playGround, new Vector(position.X - 1, position.Y));
            var rightCell = GetCell(playGround, new Vector(position.X + 1, position.Y));

            var isBottomWayFree = IsEmpty(bottomCell.Type) 
                                  && (IsEmpty(leftCell.Type) || !leftCell.IsFlagSet(0) || !leftCell.IsFlagSet(3))
                                  && (IsEmpty(rightCell.Type) || !rightCell.IsFlagSet(0) || !leftCell.IsFlagSet(2));
            
            // direct way: bottom cell is free
            if (isBottomWayFree)
            {
                return SetNewMaterialPositions(position, bottomCell, new Vector(position.X, position.Y + 1), cell.WithFlag(0, true));
            }
        }
        else if (randomDirection == 1) // right
        {
            var rightBottomCell = GetCell(playGround, new Vector(position.X + 1, position.Y + 1));
            
            // the right way is free
            if (IsEmpty(rightBottomCell.Type))
            {
                return SetNewMaterialPositions(position, rightBottomCell, new Vector(position.X + 1, position.Y + 1), cell.WithFlag(0, true).WithFlag(3, true));
            }
        }
        else // left
        {
            var leftBottomCell = GetCell(playGround, new Vector(position.X - 1, position.Y + 1));
            var leftCell = GetCell(playGround, new Vector(position.X - 1, position.Y));
            var leftOpponentCell = GetCell(playGround, new Vector(position.X - 2, position.Y));

            var isLeftWayFree = IsEmpty(leftBottomCell.Type)
                                && (IsEmpty(leftCell.Type) || !leftCell.IsFlagSet(0) || (leftCell.IsFlagSet(0) && (leftCell.IsFlagSet(2) || leftCell.IsFlagSet(3))))
                                && (IsEmpty(leftOpponentCell.Type) || !leftOpponentCell.IsFlagSet(0) || (leftCell.IsFlagSet(0) && leftCell.IsFlagSet(3)));

            if (isLeftWayFree)
            {
                return SetNewMaterialPositions(position, leftBottomCell, new Vector(position.X - 1, position.Y + 1), cell.WithFlag(0, true).WithFlag(2, true));
            }
        }
        
        return DontMove(position, cell);
    }
}