using System.Runtime.CompilerServices;
using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow;

public sealed class SandHandler(Vector dimension, uint seed = 100) : BaseMaterialHandler(dimension, seed)
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override MaterialMovement ApplyRules(PlayGround playGround, Vector position, Cell cell)
    {
        var emptyCell = new Cell(Empty, CellColor.Empty);
        
        // direct way: bottom cell is free
        var bottomCell = GetCell(playGround, new Vector(position.X, position.Y + 1));
        if (bottomCell.Type == Empty)
        {
            return new MaterialMovement(new Material(position, bottomCell), new Material(new Vector(position.X, position.Y + 1), cell));
        }
            
        var rightCell = GetCell(playGround, new Vector(position.X + 1, position.Y));
        var rightBottomCell = GetCell(playGround, new Vector(position.X + 1, position.Y + 1));
        var leftCell = GetCell(playGround, new Vector(position.X - 1, position.Y));
        var leftBottomCell = GetCell(playGround, new Vector(position.X - 1, position.Y + 1));
        var leftOpponentCell = GetCell(playGround, new Vector(position.X - 2, position.Y));

        // right way is free
        if (rightCell.Type == Empty && rightBottomCell.Type == Empty)
        {
            // the left way as well
            if (leftCell.Type == Empty && leftBottomCell.Type == Empty && (IsSolidOrEmpty(leftOpponentCell.Type) || playGround.IsMarkedCell(new Vector(position.X - 2, position.Y))))
            {
                // move by 80%
                if (WillMoveAtAll(50))
                {
                    // pseudo-random choice
                    if (WillMoveRight())
                    {
                        // go right
                        return new MaterialMovement(new Material(position, emptyCell), new Material(new Vector(position.X + 1, position.Y + 1), cell));
                    }
                    
                    // go left
                    playGround.MarkCell(position);
                    return new MaterialMovement(new Material(position, emptyCell), new Material(new Vector(position.X - 1, position.Y + 1), cell));    
                }
                    
                // dont move
                return DontMove(position, cell);
            }

            // go right by 90%
            if (WillMoveAtAll(50))
            {
                return new MaterialMovement(new Material(position, emptyCell), new Material(new Vector(position.X + 1, position.Y + 1), cell));
            }
                
            // dont move
            return DontMove(position, cell);
        }

        if (leftCell.Type == Empty && leftBottomCell.Type == Empty && (IsSolidOrEmpty(leftOpponentCell.Type) || playGround.IsMarkedCell(new Vector(position.X - 2, position.Y))))
        {
            // go left
            playGround.MarkCell(position);
            return new MaterialMovement(new Material(position, emptyCell), new Material(new Vector(position.X - 1, position.Y + 1), cell));
        }

        if (bottomCell.Type == CellType.Water)
        {
            return new MaterialMovement(new Material(position, bottomCell), new Material(new Vector(position.X, position.Y + 1), cell));
        }

        return DontMove(position, cell);
    }
}