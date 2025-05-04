using System.Runtime.CompilerServices;
using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow.MaterialHandler;

public sealed class SandHandler(Vector dimension, uint seed = 100) : BaseMaterialHandler(dimension, seed)
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override MaterialMovement? ApplyRules(PlayGround playGround, Vector position, Cell cell)
    {
        var bottomCell = GetCell(playGround, new Vector(position.X, position.Y + 1));
        
        // direct way: bottom cell is free
        if (IsEmpty(bottomCell.Type))
        {
            return SetNewMaterialPositions(position, bottomCell, new Vector(position.X, position.Y + 1), cell);
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
                                  (IsSolidOrEmpty(leftOpponentCell.Type) || leftOpponentCell.IsFlagSet(2));
        
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
                        return SetNewMaterialPositions(position, emptyCell, rightBottomPosition, cell);
                    }
                    
                    // go left
                    cell = cell.WithFlag(2, true);
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

        if (isLeftBottomWayFree)
        {
            // go left
            cell = cell.WithFlag(2, true);
            return SetNewMaterialPositions(position, emptyCell, leftBottomPosition, cell);
        }
        
        // Last option: sink into liquid
        var isBottomFreeToSink = IsLiquid(bottomCell.Type) && bottomCell.GetCounter() >= 2 && cell.GetCounter() >= 2;
        if (isBottomFreeToSink)
        {
            bottomCell = bottomCell.WithCounter(0);
            cell = cell.WithCounter(0);
            return SetNewMaterialPositions(position, bottomCell, new Vector(position.X, position.Y + 1), cell);
        }
        
        var isRightBottomWayFreeToSink = (IsEmpty(rightCell.Type) || IsLiquid(rightCell.Type) && rightCell.GetCounter() >= 2)
                                         && IsLiquid(rightBottomCell.Type) && rightBottomCell.GetCounter() >= 2 && cell.GetCounter() >= 2;
        
        if (isRightBottomWayFreeToSink)
        {
            rightBottomCell = rightBottomCell.WithCounter(0);
            cell = cell.WithCounter(0);
            return SetNewMaterialPositions(position, rightBottomCell, rightBottomPosition, cell);
        }

        var isLeftBottomWayFreeToSink = (IsEmpty(leftCell.Type) || IsLiquid(leftCell.Type) && leftCell.GetCounter() >= 2)
                                        && IsLiquid(leftBottomCell.Type) && leftBottomCell.GetCounter() >= 2
                                        && (IsSolidOrLiquidOrEmpty(leftOpponentCell.Type) || leftOpponentCell.IsFlagSet(2))
                                        && cell.GetCounter() >= 2;
        
        if (isLeftBottomWayFreeToSink)
        {
            // go left
            leftBottomCell = leftBottomCell.WithCounter(0);
            cell = cell.WithFlag(2, true);
            cell = cell.WithCounter(0);
            return SetNewMaterialPositions(position, leftBottomCell, leftBottomPosition, cell);
        }

        return DontMove(position, cell);
    }
}