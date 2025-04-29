using System.Runtime.CompilerServices;
using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow;

public sealed class SandHandler(Vector dimension, uint seed = 100) : BaseMaterialHandler(dimension, seed)
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override MaterialMovement? ApplyRules(PlayGround playGround, Vector position, Cell cell)
    {
        var bottomCell = GetCell(playGround, new Vector(position.X, position.Y + 1));
        
        // direct way: bottom cell is free
        if (IsEmpty(bottomCell.Type))
        {
            return new MaterialMovement(new Material(position, bottomCell), new Material(new Vector(position.X, position.Y + 1), cell.WithFlag(0, true)));
        }
     
        var rightPosition = new Vector(position.X + 1, position.Y);
        var rightBottomPosition = new Vector(position.X + 1, position.Y + 1);
        var leftPosition = new Vector(position.X - 1, position.Y);
        var leftBottomPosition = new Vector(position.X - 1, position.Y + 1);
        var leftOpponentPosition = new Vector(position.X - 2, position.Y);
        
        var emptyCell = new Cell(Empty, CellColor.Empty);
        var rightCell = GetCell(playGround, rightPosition);
        var rightBottomCell = GetCell(playGround, rightBottomPosition);
        var leftCell = GetCell(playGround, leftPosition);
        var leftBottomCell = GetCell(playGround, leftBottomPosition);
        var leftOpponentCell = GetCell(playGround, leftOpponentPosition);

        var isRightBottomWayFree = IsEmpty(rightCell.Type) && IsEmpty(rightBottomCell.Type);
        var isLeftBottomWayFree = IsEmpty(leftCell.Type) && IsEmpty(leftBottomCell.Type) &&
                                  (IsSolidOrEmpty(leftOpponentCell.Type) || leftOpponentCell.IsFlagSet(3));
        
        // the right way is free
        if (isRightBottomWayFree)
        {
            // the left way as well
            if (isLeftBottomWayFree)
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

        if (isLeftBottomWayFree)
        {
            // go left
            cell = cell.WithFlag(3, true);
            return new MaterialMovement(new Material(position, emptyCell), new Material(leftBottomPosition, cell.WithFlag(0, true)));
        }
        
        // Last option: sink into liquid
        if (IsLiquid(bottomCell.Type) && bottomCell.IsFlagSet(2) && cell.IsFlagSet(2))
        {
            bottomCell = bottomCell.WithFlag(2, false);
            bottomCell = bottomCell.WithFlag(1, false);
            cell = cell.WithFlag(2, false);
            cell = cell.WithFlag(1, false);
            return new MaterialMovement(new Material(position, bottomCell.WithFlag(0, true)), new Material(new Vector(position.X, position.Y + 1), cell.WithFlag(0, true))); 
        }
        
        // the right way is free
        if ((IsEmpty(rightCell.Type) || IsLiquid(rightCell.Type) && rightCell.IsFlagSet(2)) 
            && IsLiquid(rightBottomCell.Type) && rightBottomCell.IsFlagSet(2) && cell.IsFlagSet(2))
        {
            rightBottomCell = rightBottomCell.WithFlag(2, false);
            rightBottomCell = rightBottomCell.WithFlag(1, false);
            cell = cell.WithFlag(2, false);
            cell = cell.WithFlag(1, false);
            return new MaterialMovement(new Material(position, rightBottomCell.WithFlag(0, true)), new Material(rightBottomPosition, cell.WithFlag(0, true)));
        }

        // the left way is free
        if ((IsEmpty(leftCell.Type) || IsLiquid(leftCell.Type) && leftCell.IsFlagSet(2)) 
            && IsLiquid(leftBottomCell.Type) && leftBottomCell.IsFlagSet(2)
            && (IsSolidOrEmpty(leftOpponentCell.Type) || leftOpponentCell.IsFlagSet(3)))
        {
            // go left
            leftBottomCell = leftBottomCell.WithFlag(2, false);
            leftBottomCell = leftBottomCell.WithFlag(1, false);
            cell = cell.WithFlag(3, true);
            cell = cell.WithFlag(2, false);
            cell = cell.WithFlag(1, false);
            return new MaterialMovement(new Material(position, leftBottomCell.WithFlag(0, true)), new Material(leftBottomPosition, cell.WithFlag(0, true)));
        }

        return DontMove(position, cell);
    }
}