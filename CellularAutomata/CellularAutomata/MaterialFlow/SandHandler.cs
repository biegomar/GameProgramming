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

        if (leftCell.Type == Empty && leftBottomCell.Type == Empty && (IsSolidOrEmpty(leftOpponentCell.Type) || leftOpponentCell.IsFlagSet(3)))
        {
            // go left
            cell = cell.WithFlag(3, true);
            return new MaterialMovement(new Material(position, emptyCell), new Material(leftBottomPosition, cell.WithFlag(0, true)));
        }
        
        // Last option: sink into liquid
        
        // var bottomBelowNextCell = GetCell(playGround, new Vector(position.X, position.Y + 2));
        //
        // if (IsLiquid(bottomCell.Type) && IsSolidOrLiquidOrEmpty(bottomBelowNextCell.Type))
        // {
        //     return new MaterialMovement(new Material(position, bottomCell), new Material(new Vector(position.X, position.Y + 1), cell.WithFlag(0, true))); 
        // }
        
        return DontMove(position, cell);
    }
}