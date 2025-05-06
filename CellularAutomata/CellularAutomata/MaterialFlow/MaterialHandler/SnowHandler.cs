using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow.MaterialHandler;

public sealed class SnowHandler(Vector dimension, uint seed = 100) : BaseMaterialHandler(dimension, seed)
{
    public override MaterialMovement? ApplyRules(PlayGround playGround, Vector position, Cell cell)
    {
        if (cell.GetCounter() == 0)
        {
            return DontMove(position, cell);
        }
        
        var randomDirection = pseudoRandom.Next(3);
        
        if (randomDirection == 0) // right
        {
            var rightBottomCell = GetCell(playGround, new Vector(position.X + 1, position.Y + 1));
            
            if (IsEmpty(rightBottomCell.Type))
            {
                cell = cell.WithFlag(0, false);
                cell = cell.WithFlag(2, false);
                cell = cell.WithFlag(3, true);
                cell = cell.WithCounterDecrement();
                playGround.SetCell(position, cell);
                
                return SetNewMaterialPositions(position, rightBottomCell, new Vector(position.X + 1, position.Y + 1), cell);
            }
        }
        else if (randomDirection == 1) // down
        {
            var bottomCell = GetCell(playGround, new Vector(position.X, position.Y + 1));
            var leftCell = GetCell(playGround, new Vector(position.X - 1, position.Y));

            var isBottomWayFree = IsEmpty(bottomCell.Type) 
                                  && (IsEmpty(leftCell.Type) || !leftCell.IsMovingRight());
            
            if (isBottomWayFree)
            {
                cell = cell.WithFlag(0, true);
                cell = cell.WithFlag(2, false);
                cell = cell.WithFlag(3, false);
                cell = cell.WithCounterDecrement();
                playGround.SetCell(position, cell);
                
                return SetNewMaterialPositions(position, bottomCell, new Vector(position.X, position.Y + 1), cell);
            }
        }
        else // left
        {
            var leftBottomCell = GetCell(playGround, new Vector(position.X - 1, position.Y + 1));
            var leftCell = GetCell(playGround, new Vector(position.X - 1, position.Y));
            var leftOpponentCell = GetCell(playGround, new Vector(position.X - 2, position.Y));

            var isLeftWayFree = IsEmpty(leftBottomCell.Type)
                                && (IsEmpty(leftCell.Type) || !leftCell.IsMoving())
                                && (IsEmpty(leftOpponentCell.Type) || leftOpponentCell.IsMovingLeft() || leftOpponentCell.IsMoving());

            if (isLeftWayFree)
            {
                cell = cell.WithFlag(0, false);
                cell = cell.WithFlag(3, false);
                cell = cell.WithFlag(2, true);
                cell = cell.WithCounterDecrement();
                playGround.SetCell(position, cell);
                
                return SetNewMaterialPositions(position, leftBottomCell, new Vector(position.X - 1, position.Y + 1), cell);
            }
        }
        
        return DontMove(position, cell);
    }
}