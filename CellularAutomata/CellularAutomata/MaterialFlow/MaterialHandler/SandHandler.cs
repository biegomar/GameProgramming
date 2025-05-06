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
        
        var rightCell = GetCell(playGround, new Vector(position.X + 1, position.Y));
        var rightBottomCell = GetCell(playGround, new Vector(position.X + 1, position.Y + 1));
        var leftCell = GetCell(playGround, new Vector(position.X - 1, position.Y));
        var leftBottomCell = GetCell(playGround, new Vector(position.X - 1, position.Y + 1));
        var leftOpponentCell = GetCell(playGround, new Vector(position.X - 2, position.Y));

        var isRightBottomWayFree = IsEmpty(rightCell.Type) && IsEmpty(rightBottomCell.Type);
        var isLeftBottomWayFree = IsEmpty(leftCell.Type) && IsEmpty(leftBottomCell.Type) &&
                                  (IsSolidOrEmpty(leftOpponentCell.Type) || leftOpponentCell.IsMovingLeft());
        
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
                        return SetNewMaterialPositions(position, EmptyCell, new Vector(position.X + 1, position.Y + 1), cell);
                    }
                    
                    // go left
                    cell = cell.WithFlag(2, true);
                    playGround.SetCell(position, cell);
                    return SetNewMaterialPositions(position, EmptyCell, new Vector(position.X - 1, position.Y + 1), cell);
                }
                    
                // dont move
                return DontMove(position, cell);
            }

            // go right by 90%
            if (WillMoveAtAll(90))
            {
                return SetNewMaterialPositions(position, EmptyCell, new Vector(position.X + 1, position.Y + 1), cell);
            }
                
            // dont move
            return DontMove(position, cell);
        }

        if (isLeftBottomWayFree)
        {
            // go left
            cell = cell.WithFlag(2, true);
            playGround.SetCell(position, cell);
            return SetNewMaterialPositions(position, EmptyCell, new Vector(position.X - 1, position.Y + 1), cell);
        }
        
        // Last option: sink into liquid
        var isBottomFreeToSink = IsLiquid(bottomCell.Type) && bottomCell.GetCounter() >= 2 && cell.GetCounter() >= 2;
        if (isBottomFreeToSink)
        {
            return SetNewMaterialPositions(position, bottomCell, new Vector(position.X, position.Y + 1), cell);
        }
        
        var isRightBottomWayFreeToSink = cell.GetCounter() >= 2 
                                         && (IsEmpty(rightCell.Type) || IsLiquid(rightCell.Type) && rightCell.GetCounter() >= 2)
                                         && IsLiquid(rightBottomCell.Type) && rightBottomCell.GetCounter() >= 2;
        
        if (isRightBottomWayFreeToSink)
        {
            return SetNewMaterialPositions(position, rightBottomCell, new Vector(position.X + 1, position.Y + 1), cell);
        }

        var isLeftBottomWayFreeToSink = cell.GetCounter() >= 2 
                                        && (IsEmpty(leftCell.Type) || IsLiquid(leftCell.Type) && leftCell.GetCounter() >= 2)
                                        && IsLiquid(leftBottomCell.Type) && leftBottomCell.GetCounter() >= 2
                                        && (IsSolidOrLiquidOrEmpty(leftOpponentCell.Type) || leftOpponentCell.IsMovingLeft());
        
        if (isLeftBottomWayFreeToSink)
        {
            // go left
            cell = cell.WithFlag(2, true);
            playGround.SetCell(position, cell);
            return SetNewMaterialPositions(position, leftBottomCell, new Vector(position.X - 1, position.Y + 1), cell);
        }

        return DontMove(position, cell);
    }
}