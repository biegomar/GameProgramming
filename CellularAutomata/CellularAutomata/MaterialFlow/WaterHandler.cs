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
            return new MaterialMovement(new Material(position, bottomCell), new Material(new Vector(position.X, position.Y + 1), cell.WithFlag(0, true)));
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
            if (leftCell.Type == Empty && leftBottomCell.Type == Empty && (IsSolidOrEmpty(leftOpponentCell.Type) || leftOpponentCell.IsFlagSet(3)))
            {
                // move by 80%
                if (WillMoveAtAll(80))
                {
                    // pseudo-random choice
                    if (WillMoveRight())
                    {
                        // go right
                        return new MaterialMovement(new Material(position, emptyCell), new Material(rightBottomPosition, cell.WithFlag(0, true)));
                    }
                    
                    // go left
                    cell = cell.WithFlag(3, true);
                    return new MaterialMovement(new Material(position, emptyCell), new Material(leftBottomPosition, cell.WithFlag(0, true)));    
                }
                    
                // dont move
                return DontMove(position, cell);
            }

            // go right by 90%
            if (WillMoveAtAll(90))
            {
                return new MaterialMovement(new Material(position, emptyCell), new Material(rightBottomPosition, cell.WithFlag(0, true)));
            }
                
            // dont move
            return DontMove(position, cell);
        }

        // the left bottom way is free 
        if (IsEmpty(leftCell.Type) && IsEmpty(leftBottomCell.Type) && (IsSolidOrEmpty(leftOpponentCell.Type) || leftOpponentCell.IsFlagSet(3)))
        {
            // go left
            cell = cell.WithFlag(3, true);
            return new MaterialMovement(new Material(position, emptyCell), new Material(leftBottomPosition, cell.WithFlag(0, true)));
        }
        
        //Sliding
        
        var topCell = GetCell(playGround, new Vector(position.X, position.Y - 1));
        var topRightCell = GetCell(playGround, new Vector(position.X + 1, position.Y - 1));
        var topRightOpponentCell = GetCell(playGround, new Vector(position.X + 2, position.Y - 1));
        var topLeftCell = GetCell(playGround, new Vector(position.X - 1, position.Y - 1));
        var topLeftOpponentCell = GetCell(playGround, new Vector(position.X - 2, position.Y - 1));
        
        // the right way is free
        if (!cell.IsFlagSet(4) && IsEmpty(rightCell.Type) && IsSolidOrEmpty(topCell.Type) && IsSolidOrEmpty(topRightCell.Type) && IsSolidOrEmpty(topRightOpponentCell.Type))
        {
            return new MaterialMovement(new Material(position, emptyCell), new Material(rightPosition, cell.WithFlag(0, true)));
        }

        // the left way is free
        if (IsEmpty(leftCell.Type)
            && IsSolidOrEmpty(topCell.Type)
            && (IsSolidOrEmpty(leftOpponentCell.Type) || leftOpponentCell.IsFlagSet(2))
            && IsSolidOrEmpty(topLeftCell.Type)
            && IsSolidOrEmpty(topLeftOpponentCell.Type))
        {
            cell = cell.WithFlag(4, true);
            return new MaterialMovement(new Material(position, emptyCell), new Material(leftPosition, cell.WithFlag(0, true)));
        }

        return DontMove(position, cell with { Flags = 0 });
    }
}