using Mazes.Contracts;
using Mazes.Contracts.Cells;
using NearlyRogue.Core.FightSystems;
using NearlyRogue.Core.Movement;
using NearlyRogue.Core.Numerics;

namespace NearlyRogue.Cli;

public class PlayerMovement : IMovement<ICreature<char>>
{
    private readonly Landscape<ICreature<char>> landscape;
    private ICreature<char> item;
    
    public PlayerMovement(Landscape<ICreature<char>> landscape, ICreature<char> player, Vector playerPosition)
    {
        this.landscape = landscape;
        this.item = player;
        this.ActualPosition = playerPosition;
        SetAndDrawItem();
    }

    public Vector ActualPosition { get; set; }

    public void MoveTo(Vector position)
    {
        var newPosition = new Vector(this.ActualPosition.X + position.X, this.ActualPosition.Y + position.Y,
            this.ActualPosition.Z + position.Z);

        var isNewPositionInGrid = newPosition.X >= 0 && newPosition.X < this.landscape.Width && newPosition.Y >= 0 &&
                                  newPosition.Y < this.landscape.Height;


        var isNewCellLinked = isNewPositionInGrid && (GetCellByColumnAndRow(this.ActualPosition.X, this.ActualPosition.Y)
            .LinkedCells
            .Contains(GetCellByColumnAndRow(newPosition.X, newPosition.Y)));
        
        if (isNewPositionInGrid && isNewCellLinked)
        {
            this.landscape.ClearCellItem(new CellVector(this.ActualPosition.X, this.ActualPosition.Y, 0));
            
            this.ActualPosition = new Vector(this.ActualPosition.X + position.X, this.ActualPosition.Y + position.Y,
                this.ActualPosition.Z + position.Z);
        
            SetAndDrawItem();
        }
    }

    private void SetAndDrawItem()
    {
        landscape.SetCellItem(new CellItem<ICreature<char>>(this.item, 
            new CellVector(
                this.ActualPosition.X,
                this.ActualPosition.Y,0)));
        landscape.DrawCellItems();
    }
    
    private Cell<ICreature<char>> GetCellByColumnAndRow(int column, int row)
    {
        return this.landscape.Cells.Single(cell => cell.X == column && cell.Y == row);
    }
}