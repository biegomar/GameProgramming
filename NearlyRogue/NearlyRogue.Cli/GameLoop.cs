using System.Numerics;
using AldousBroderMaze;
using Mazes.Contracts;
using NearlyRogue.Core.Amors;
using NearlyRogue.Core.Dices;
using NearlyRogue.Core.FightSystems;
using NearlyRogue.Core.Monsters;
using NearlyRogue.Core.Players;
using NearlyRogue.Core.Weapons;
using Vector = NearlyRogue.Core.Numerics.Vector;

namespace NearlyRogue.Cli;

public class GameLoop
{
    private readonly Random random = new ();
    private readonly MonsterFactory<char> monsterFactory = new (Preparations.GetIcons());
    private readonly WeaponFactory weaponFactory = new();
    private readonly ArmorFactory armorFactory = new();
    
    private Player<char> player;
    private IList<Monster<char>> monsters;
    private FightSystem<char> fightSystem;
    private Landscape<ICreature<char>> landscape;
    private PlayerMovement playerMovement;
    private MonsterAI monsterMovement;

    private bool playAnotherGame = true;
    private bool isGameFinished = false;
    
    public void Run()
    {
        do
        {
            InitGame();
            do
            {
                //this.fightSystem.BattleOneRound(player, monster);
                this.playerMovement.MoveTo(GetInput());
                this.monsterMovement.MoveTo(default);
            } while (!isGameFinished);

            this.playAnotherGame = false;
        } while (playAnotherGame);
    }

    private void InitGame()
    {
        int dimX = 10;
        int dimY = 10;
        var dimension = new CellVector(dimX, dimY, 0);
        this.landscape = new Landscape<ICreature<char>>(dimension, new AldousBroderMazeGenerator<ICreature<char>>(null),
            new ConsoleMazePrinter<ICreature<char>>(), "AldousBroder");
        
        
        this.player = Preparations.CreatePlayer(landscape);
        this.playerMovement = new PlayerMovement(this.landscape, player, new Vector(8, 8, 0));

        this.monsters = new List<Monster<char>>();
        var monster = monsterFactory.CreateMonster(MonsterRace.Kestrel);
        var x = this.random.Next(0, dimX);
        var y = this.random.Next(0, dimY);
        this.monsterMovement = new MonsterAI(this.landscape, monster, new Vector(x, y, 0));
        this.monsters.Add(monster);
        
        this.fightSystem = new FightSystem<char>();
        
        landscape.Draw(CellVector.Zero);
        
        landscape.DrawCellItems();
    }

    private Vector GetInput()
    {
        Vector result;
        var key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.LeftArrow:
                result = new Vector(-1, 0, 0);
                break;
            case ConsoleKey.UpArrow:
                result = new Vector(0, -1, 0);
                break;
            case ConsoleKey.RightArrow:
                result = new Vector(1, 0, 0);
                break;
            case ConsoleKey.DownArrow:
                result = new Vector(0, 1, 0);
                break;
            case ConsoleKey.Q:
                result = new Vector(0, 0, 0);
                isGameFinished = true;
                break;
            default:
                result = new Vector(0, 0, 0);
                break;
        }

        return result;
    }
}