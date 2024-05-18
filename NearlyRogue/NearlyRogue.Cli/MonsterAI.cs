using System.Diagnostics;
using FiniteStateMachine.EventArgs;
using MazePathFinder;
using Mazes.Contracts;
using Mazes.Contracts.PathFinding;
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
    private IPathFinder<ICreature<char>> pathFinder;
    private bool Attack;
    
    public Vector ActualPosition { get; set; }
    
    public MonsterAI(Maze<ICreature<char>> maze, ICreature<char> monster, Vector monsterPosition)
    {
        this.maze = maze;
        this.item = monster;
        this.ActualPosition = monsterPosition;
        this.pathFinder = new PathFinderForMaze<ICreature<char>>(this.maze);
        this.Attack = false;
        
        this.SetAndDrawItem();
        
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        var idleState = new State();
        
        var seekState = new State();
        seekState.Update += SeekPlayerUpdate;
        
        var attackState = new State();
        attackState.Enter += EnterAttack;
        attackState.Update += UpdateAttack;

        var transitToSeekState = new Transition(this.IsPlayerInReach, seekState);
        idleState.AddTransition(transitToSeekState);
        
        var transitToAttackState = new Transition(() => Attack, attackState);
        seekState.AddTransition(transitToAttackState);

        var transitFromAttackToSeekState = new Transition(() => !this.Attack, seekState);
        attackState.AddTransition(transitFromAttackToSeekState);
        
        
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
            new CellVector(
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
        var playerPosition = GetPlayerPosition();
        var path = this.pathFinder.GetShortestPath(new CellVector(this.ActualPosition.X, this.ActualPosition.Y, 0),
            playerPosition);
        if (path.Count > 1)
        {
            var nextCell = path[1];
            if (nextCell.X == playerPosition.X && nextCell.Y == playerPosition.Y)
            {
                this.Attack = true;
            }
            else
            {
                this.maze.ClearCellItem(new CellVector(this.ActualPosition.X, this.ActualPosition.Y, 0));
            
                this.ActualPosition = new Vector(nextCell.X, nextCell.Y, 0);
            }
            
            SetAndDrawItem();
        }
    }

    private void EnterAttack(object? sender, EnterEventArgs eventArgs)
    {
        Debug.WriteLine("Enter Attack!");
    }

    private void UpdateAttack(object? sender, UpdateEventArgs eventArgs)
    {
        var playerPosition = GetPlayerPosition();
        var path = this.pathFinder.GetShortestPath(new CellVector(this.ActualPosition.X, this.ActualPosition.Y, 0),
            playerPosition);

        if (path.Count > 1)
        {
            var nextCell = path[1];
            if (nextCell.X != playerPosition.X || nextCell.Y != playerPosition.Y)
            {
                this.Attack = false;
            }
            else
            {
                Debug.WriteLine("Update Attack!");
            }
        }
        else
        {
            this.Attack = false;
        }
    }

    private CellVector GetPlayerPosition()
    {
        var playerCell = this.maze.Cells.Cast<Cell<ICreature<char>>>().FirstOrDefault(c => c.Item is Player<char>);
        if (playerCell != null)
        {
            return new CellVector(playerCell.X, playerCell.Y, 0);
        }
        else
        {
            return null;
        }
    }
}