using FiniteStateMachine.EventArgs;
using Mazes.Contracts;
using NearlyRogue.Core.FightSystems;
using NearlyRogue.Core.Movement;
using NearlyRogue.Core.Numerics;
using NearlyRogue.Core.Players;

namespace NearlyRogue.Cli;

using FiniteStateMachine;

public class MonsterAI: IMovement<ICreature<char>>
{
    private readonly Maze<ICreature<char>> maze;
    private Random random = new ();
    private ICreature<char> item;
    private FiniteStateMachine fsm;
    
    public Vector ActualPosition { get; set; }
    
    public MonsterAI(Maze<ICreature<char>> maze, ICreature<char> monster, Vector monsterPosition)
    {
        this.maze = maze;
        this.item = monster;
        this.ActualPosition = monsterPosition;
        this.SetAndDrawItem();
        
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        var idleState = new State();
        
        var seekState = new State();
        seekState.Update += SeekPlayerUpdate;
        
        var attackState = new State(); 
            
        bool shouldChangeToAttackState = true;

        var transitToSeekState = new Transition(this.IsPlayerInReach, seekState);
        var transitToAttackState = new Transition(() => shouldChangeToAttackState, attackState);
        idleState.AddTransition(transitToSeekState);
        
        fsm = new FiniteStateMachine(idleState);
        fsm.AddState(seekState);
        
        fsm.StartMachine();
        fsm.UpdateMachine();
    }

    public void MoveTo(Vector position)
    {
        fsm.UpdateMachine();
    }
    
    private void SetAndDrawItem()
    {
        maze.SetCellItem(new CellItem<ICreature<char>>(this.item, 
            new MazeVector(
                this.ActualPosition.X,
                this.ActualPosition.Y,0)));
        maze.DrawCellItems();
    }

    private bool IsPlayerInReach()
    {
        var cell = this.maze.Cells[this.ActualPosition.X, this.ActualPosition.Y]!;
        foreach (var neighbour in cell.Neighbours.Values.Where(x => x?.Item != null))
        {
            if (neighbour?.Item is Player<char> player)
            {
                return true;
            }
        }

        return false;
    }
    
    private void SeekPlayerUpdate(object? sender, UpdateEventArgs eventArgs)
    {
        var cell = this.maze.Cells[this.ActualPosition.X, this.ActualPosition.Y]!;
        if (cell.LinkedCells.Any())
        {
            var randomCellIndex = this.random.Next(0, cell.LinkedCells.Count);
            var randomCell = cell.LinkedCells[randomCellIndex];
            
            this.maze.ClearCellItem(new MazeVector(this.ActualPosition.X, this.ActualPosition.Y, 0));
            this.ActualPosition = new Vector(randomCell.X, randomCell.Y, 0);
        
            SetAndDrawItem();
        }
        
    }
}