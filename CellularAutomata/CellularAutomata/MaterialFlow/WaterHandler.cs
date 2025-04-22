using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow;

public sealed class WaterHandler(Vector dimension, uint seed = 100) : BaseMaterialHandler(dimension, seed)
{
    public override MaterialMovement? ApplyRules(PlayGround playGround, Vector position, Cell cell)
    {
        if (playGround.IsMarkedCell(position)) return null;
        
        var bottomCell = GetCell(playGround, new Vector(position.X, position.Y + 1));
        
        // direct way: bottom cell is free
        if (bottomCell.Type == Empty)
        {
            return new MaterialMovement(new Material(position, bottomCell), new Material(new Vector(position.X, position.Y + 1), cell));
        }
        
        var rightPosition = new Vector(position.X + 1, position.Y);
        var rightBottomPosition = new Vector(position.X + 1, position.Y + 1);
        var leftPosition = new Vector(position.X - 1, position.Y);
        var leftBottomPosition = new Vector(position.X - 1, position.Y + 1);
        var leftOpponentPosition = new Vector(position.X - 2, position.Y);
        
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
            if (leftCell.Type == Empty && leftBottomCell.Type == Empty && (IsSolidOrEmpty(leftOpponentCell.Type) || playGround.IsMarkedCell(leftOpponentPosition)))
            {
                // move by 80%
                if (WillMoveAtAll(80))
                {
                    // pseudo-random choice
                    if (WillMoveRight())
                    {
                        // go right
                        return new MaterialMovement(new Material(position, emptyCell), new Material(rightBottomPosition, cell));
                    }
                    
                    // go left
                    playGround.MarkCell(position);
                    return new MaterialMovement(new Material(position, emptyCell), new Material(leftBottomPosition, cell));    
                }
                    
                // dont move
                return DontMove(position, cell);
            }

            // go right by 90%
            if (WillMoveAtAll(90))
            {
                return new MaterialMovement(new Material(position, emptyCell), new Material(rightBottomPosition, cell));
            }
                
            // dont move
            return DontMove(position, cell);
        }

        // the left bottom way is free 
        if (IsEmpty(leftCell.Type) && IsEmpty(leftBottomCell.Type) && (IsSolidOrEmpty(leftOpponentCell.Type) || playGround.IsMarkedCell(leftOpponentPosition)))
        {
            // go left
            playGround.MarkCell(position);
            return new MaterialMovement(new Material(position, emptyCell), new Material(leftBottomPosition, cell));
        }
        
        var topCell = GetCell(playGround, new Vector(position.X, position.Y - 1));

        if (IsSand(topCell.Type))
        {
            
        }
        
        //Sliding
        
        //var topRightPosition = new Vector(position.X + 1, position.Y - 1);
        //var topLeftPosition = new Vector(position.X - 1, position.Y - 1);
        //var topPosition = new Vector(position.X, position.Y - 1);
        
        var topRightCell = GetCell(playGround, new Vector(position.X + 1, position.Y - 1));
        var topRightOpponentCell = GetCell(playGround, new Vector(position.X + 2, position.Y - 1));
        var topLeftCell = GetCell(playGround, new Vector(position.X - 1, position.Y - 1));
        var topLeftOpponentCell = GetCell(playGround, new Vector(position.X - 2, position.Y - 1));
        
        // the right way is free
        if (IsEmpty(rightCell.Type) && IsSolidOrEmpty(topCell.Type) && IsSolidOrEmpty(topRightCell.Type) && IsSolidOrEmpty(topRightOpponentCell.Type))
        {
            // the left way as well
            if (IsEmpty(leftCell.Type) 
                && IsSolidOrEmpty(topCell.Type) 
                && (IsSolidOrEmpty(leftOpponentCell.Type) || playGround.IsMarkedCell(leftOpponentPosition)) 
                && IsSolidOrEmpty(topLeftCell.Type) 
                && IsSolidOrEmpty(topLeftOpponentCell.Type))
            {
                // move by 80%
                if (WillMoveAtAll(80))
                {
                    // pseudo-random choice
                    if (WillMoveRight())
                    {
                        // go right
                        return new MaterialMovement(new Material(position, emptyCell), new Material(rightPosition, cell));
                    }
                    
                    // go left
                    playGround.MarkCell(leftPosition);
                    return new MaterialMovement(new Material(position, emptyCell), new Material(leftPosition, cell));    
                }
                    
                return DontMove(position, cell);
            }
            
            // go right by 90%
            if (WillMoveAtAll(90))
            {
                return new MaterialMovement(new Material(position, emptyCell), new Material(rightPosition, cell));
            }
        }

        if (IsEmpty(leftCell.Type) 
            && IsSolidOrEmpty(topCell.Type) 
            && (IsSolidOrEmpty(leftOpponentCell.Type) || playGround.IsMarkedCell(leftOpponentPosition)) 
            && IsSolidOrEmpty(topLeftCell.Type) 
            && IsSolidOrEmpty(topLeftOpponentCell.Type))
        {
            // go left
            playGround.MarkCell(leftPosition);
            return new MaterialMovement(new Material(position, emptyCell), new Material(leftPosition, cell));
        }
        
        return DontMove(position, cell);
    }
}