using Mazes.Contracts;
using NearlyRogue.Core.FightSystems;
using NearlyRogue.Core.Movement;
using NearlyRogue.Core.Numerics;

namespace NearlyRogue.Cli;

public class PlayerMovement : IMovement<ICreature<char>>
{
    private readonly Maze<ICreature<char>> maze;
    private ICreature<char> item;
    
    public PlayerMovement(Maze<ICreature<char>> maze, ICreature<char> player, Vector playerPosition)
    {
        this.maze = maze;
        this.item = player;
        this.ActualPosition = playerPosition;
        SetAndDrawItem();
    }

    public Vector ActualPosition { get; set; }

    public void MoveTo(Vector position)
    {
        var newPosition = new Vector(this.ActualPosition.X + position.X, this.ActualPosition.Y + position.Y,
            this.ActualPosition.Z + position.Z);

        var isNewPositionInGrid = newPosition.X >= 0 && newPosition.X < this.maze.Width && newPosition.Y >= 0 &&
                                  newPosition.Y < this.maze.Height;


        var isNewCellLinked = isNewPositionInGrid && (this.maze.Cells[this.ActualPosition.X, this.ActualPosition.Y]!
            .LinkedCells
            .Contains(this.maze.Cells[newPosition.X, newPosition.Y]!));
        
        if (isNewPositionInGrid && isNewCellLinked)
        {
            this.maze.ClearCellItem(new CellVector(this.ActualPosition.X, this.ActualPosition.Y, 0));
            
            this.ActualPosition = new Vector(this.ActualPosition.X + position.X, this.ActualPosition.Y + position.Y,
                this.ActualPosition.Z + position.Z);
        
            SetAndDrawItem();
        }
    }

    private void SetAndDrawItem()
    {
        maze.SetCellItem(new CellItem<ICreature<char>>(this.item, 
            new CellVector(
                this.ActualPosition.X,
                this.ActualPosition.Y,0)));
        maze.DrawCellItems();
    }
}