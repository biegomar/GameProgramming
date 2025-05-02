using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow;

public sealed class WaterHandler(Vector dimension, uint seed = 100) : BaseMaterialHandler(dimension, seed)
{
    public override MaterialMovement? ApplyRules(PlayGround playGround, Vector position, Cell cell)
    {
        var bottomCell = GetCell(playGround, new Vector(position.X, position.Y + 1));
        
        // direct way: bottom cell is free
        if (bottomCell.Type == Empty)
        {
            return SetNewMaterialPositions(position, bottomCell, new Vector(position.X, position.Y + 1), cell);
        }
        
        var rightPosition = new Vector(position.X + 1, position.Y);
        var rightBottomPosition = new Vector(position.X + 1, position.Y + 1);
        var leftPosition = new Vector(position.X - 1, position.Y);
        var leftBottomPosition = new Vector(position.X - 1, position.Y + 1);
        
        var emptyCell = new Cell(Empty, CellColor.Empty);
        var rightCell = GetCell(playGround, rightPosition);
        var rightBottomCell = GetCell(playGround, new Vector(position.X + 1, position.Y + 1));
        var leftCell = GetCell(playGround, leftPosition);
        var leftBottomCell = GetCell(playGround, new Vector(position.X - 1, position.Y + 1));
        var leftOpponentCell = GetCell(playGround, new Vector(position.X - 2, position.Y));
        
        // the right way is free
        if (rightCell.Type == Empty && rightBottomCell.Type == Empty)
        {
            // the left way as well
            if (leftCell.Type == Empty && leftBottomCell.Type == Empty && (IsSolidOrEmpty(leftOpponentCell.Type) || leftOpponentCell.IsFlagSet(0)))
            {
                // move by 80%
                if (WillMoveAtAll(80))
                {
                    // pseudo-random choice
                    if (WillMoveRight())
                    {
                        // go right
                        return SetNewMaterialPositions(position, emptyCell, rightBottomPosition, cell);
                    }
                    
                    // go left
                    cell = cell.WithFlag(0, true);
                    return SetNewMaterialPositions(position, emptyCell, leftBottomPosition, cell);
                }
                    
                // dont move
                return DontMove(position, cell);
            }

            // go right by 90%
            if (WillMoveAtAll(90))
            {
                return SetNewMaterialPositions(position, emptyCell, rightBottomPosition, cell);
            }
                
            // dont move
            return DontMove(position, cell);
        }

        // the left bottom way is free 
        if (IsEmpty(leftCell.Type) && IsEmpty(leftBottomCell.Type) && (IsSolidOrEmpty(leftOpponentCell.Type) || leftOpponentCell.IsFlagSet(0)))
        {
            // go left
            cell = cell.WithFlag(0, true);
            return SetNewMaterialPositions(position, emptyCell, leftBottomPosition, cell);
        }
        
        //Sliding
        
        var rightOpponentCell = GetCell(playGround, new Vector(position.X + 2, position.Y));
        var topCell = GetCell(playGround, new Vector(position.X, position.Y - 1));
        var topRightCell = GetCell(playGround, new Vector(position.X + 1, position.Y - 1));
        var topRightOpponentCell = GetCell(playGround, new Vector(position.X + 2, position.Y - 1));
        var topLeftCell = GetCell(playGround, new Vector(position.X - 1, position.Y - 1));
        var topLeftOpponentCell = GetCell(playGround, new Vector(position.X - 2, position.Y - 1));

        var isRightWayFree = !cell.IsFlagSet(4) 
                             && IsSolidOrEmpty(topCell.Type) 
                             && IsEmpty(rightCell.Type) 
                             && IsSolidOrEmpty(topRightCell.Type) 
                             && IsSolidOrEmpty(topRightOpponentCell.Type);
        
        // the right way is free
        if (isRightWayFree)
        {
            cell = cell.WithFlag(0, false);
            return SetNewMaterialPositions(position, emptyCell, rightPosition, cell);
        }
        
        // flag to move only left until blocked
        cell = cell.WithFlag(4, true);

        // the left way is free
        var isLeftWayFree = cell.IsFlagSet(4)
                            && IsSolidOrEmpty(topCell.Type)
                            && IsEmpty(leftCell.Type)
                            && IsSolidOrEmpty(topLeftCell.Type)
                            && IsSolidOrEmpty(topLeftOpponentCell.Type)
                            && (IsNonSlidingOrEmpty(leftOpponentCell.Type) || IsLiquid(leftOpponentCell.Type) && leftOpponentCell.IsFlagSet(4));
        
        if (isLeftWayFree)
        {
            cell = cell.WithFlag(0, true);
            return SetNewMaterialPositions(position, emptyCell, leftPosition, cell);
        }
        
        // reset flag to enable moving right
        cell = cell.WithFlag(4, false);
        
        // let other materials sink in.
        if (topCell.IsFlagSet(2) && cell.IsFlagSet(2))
        {
            return null;
        }

        return DontMove(position, cell);
    }
}