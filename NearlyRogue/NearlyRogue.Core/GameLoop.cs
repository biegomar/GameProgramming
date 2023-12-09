using NearlyRogue.Core.Dices;
using NearlyRogue.Core.FightSystems;
using NearlyRogue.Core.Monsters;
using NearlyRogue.Core.Players;

namespace NearlyRogue.Core;

public class GameLoop
{
    private readonly MonsterFactory monsterFactory = new ();
    private Player player;
    private Monster monster;
    private FightSystem fightSystem;
    
    public void Run()
    {
        do
        {
            InitGame();
            do
            {
                this.fightSystem.BattleOneRound(player, monster);
            } while (true);
        } while (true);
    }

    private void InitGame()
    {
        this.player = CreatePlayer();
        this.monster = monsterFactory.CreateMonster(MonsterRace.Kestrel);
        this.fightSystem = new FightSystem();
    }

    private Player CreatePlayer()
    {
        DiceThrow diceThrow = new(1, new Dice(DiceType.D4));
        
        return new Player(new ExperienceCalculator())
        {
            Name = "atogeib",
            Strength = 16,
            Experience = 0,
            ExperienceLevel = 1,
            AmorClass = 10,
            HitPoints = 12,
            MaxHitPoints = 12,
            Damage = new List<DiceThrow>() { diceThrow}
        };
    }
}