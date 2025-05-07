using System.Runtime.CompilerServices;
using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow.MaterialHandler;

public sealed class WaterHandler(Vector dimension, uint seed = 100) : BaseMaterialHandler(dimension, seed)
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override MaterialMovement? ApplyRules(PlayGround playGround, Vector position, Cell cell)
    {
        var bottomCell = GetCell(playGround, new Vector(position.X, position.Y + 1));
        
        // direct way: bottom cell is free
        if (bottomCell.Type == Empty)
        {
            return SetNewMaterialPositions(position, bottomCell, new Vector(position.X, position.Y + 1), cell);
        }
        
        var isRightCellEmpty = IsEmpty(GetCell(playGround, new Vector(position.X + 1, position.Y)).Type);
        var isLeftCellEmpty = IsEmpty(GetCell(playGround, new Vector(position.X - 1, position.Y)).Type);  
        var isRightBottomCellEmpty = IsEmpty(GetCell(playGround, new Vector(position.X + 1, position.Y + 1)).Type);
        var isLeftBottomCellEmpty = IsEmpty(GetCell(playGround, new Vector(position.X - 1, position.Y + 1)).Type);
        
        var leftOpponentCell = GetCell(playGround, new Vector(position.X - 2, position.Y));
        
        // the right way is free
        if (isRightCellEmpty && isRightBottomCellEmpty)
        {
            // the left way as well
            if (isLeftCellEmpty && isLeftBottomCellEmpty && (IsSolidOrEmpty(leftOpponentCell.Type) || leftOpponentCell.IsMovingLeft()))
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

            // go right
            return SetNewMaterialPositions(position, EmptyCell, new Vector(position.X + 1, position.Y + 1), cell);
        }

        // the left bottom way is free 
        if (isLeftCellEmpty && isLeftBottomCellEmpty && (IsSolidOrEmpty(leftOpponentCell.Type) || leftOpponentCell.IsMovingLeft()))
        {
            // go left
            cell = cell.WithFlag(2, true);
            playGround.SetCell(position, cell);
            return SetNewMaterialPositions(position, EmptyCell, new Vector(position.X - 1, position.Y + 1), cell);
        }
        
        //Sliding
        var topCell = GetCell(playGround, new Vector(position.X, position.Y - 1));
        var topRightCell = GetCell(playGround, new Vector(position.X + 1, position.Y - 1));
        var topRightOpponentCell = GetCell(playGround, new Vector(position.X + 2, position.Y - 1));
        var topLeftCell = GetCell(playGround, new Vector(position.X - 1, position.Y - 1));
        var topLeftOpponentCell = GetCell(playGround, new Vector(position.X - 2, position.Y - 1));

        var isRightWayFree = !cell.IsSlidingLeft() 
                             && isRightCellEmpty
                             && IsSolidOrEmpty(topCell.Type) 
                             && IsSolidOrEmpty(topRightCell.Type) 
                             && IsSolidOrEmpty(topRightOpponentCell.Type);
        
        // the right way is free
        if (isRightWayFree)
        {
            cell = cell.WithFlag(2, false);
            return SetNewMaterialPositions(position, EmptyCell, new Vector(position.X + 1, position.Y), cell);
        }
        
        // flag to move only left until blocked
        cell = cell.WithFlag(1, true);

        // the left way is free
        var isLeftWayFree = isLeftCellEmpty
                            && IsSolidOrEmpty(topCell.Type)
                            && IsSolidOrEmpty(topLeftCell.Type)
                            && IsSolidOrEmpty(topLeftOpponentCell.Type)
                            && (IsNonSlidingOrEmpty(leftOpponentCell.Type) || IsLiquid(leftOpponentCell.Type) && leftOpponentCell.IsSlidingLeft());
        
        if (isLeftWayFree)
        {
            cell = cell.WithFlag(2, true);
            return SetNewMaterialPositions(position, EmptyCell, new Vector(position.X - 1, position.Y), cell);
        }
        
        // let other materials sink in.
        if (topCell.GetCounter() >= SinkInCounter && cell.GetCounter() >= SinkInCounter)
        {
            return null;
        }
        
        // or let it freeze from the left side.
        var leftCell = GetCell(playGround, new Vector(position.X - 1, position.Y));
        if (IsIce(leftCell.Type) && leftCell.GetCounter() >= SinkInCounter && cell.GetCounter() >= SinkInCounter)
        {
            return null;
        }
        
        // reset flag to enable moving right
        cell = cell.WithFlag(1, false);

        return DontMove(position, cell);
    }
}