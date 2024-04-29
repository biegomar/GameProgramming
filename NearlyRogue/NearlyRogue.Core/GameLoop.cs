using NearlyRogue.Core.Amors;
using NearlyRogue.Core.Dices;
using NearlyRogue.Core.FightSystems;
using NearlyRogue.Core.Monsters;
using NearlyRogue.Core.Players;
using NearlyRogue.Core.Weapons;

namespace NearlyRogue.Core;

public class GameLoop<T>
{
    private readonly Random random = new ();
    private readonly MonsterFactory<T> monsterFactory = new ();
    private readonly WeaponFactory weaponFactory = new();
    private readonly ArmorFactory armorFactory = new();
    
    private Player<T> player;
    private Monster<T> monster;
    private FightSystem<T> fightSystem;
    
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
        this.fightSystem = new FightSystem<T>();
    }

    private Player<T> CreatePlayer()
    {
        DiceThrow diceThrow = new(1, new Dice(DiceType.D4));
        var mace = this.weaponFactory.CreateWeapon(WeaponType.Mace);
        mace.AdditionalHit = 1;
        mace.AdditionalDamage = 1;
        mace.Flags |= WeaponFlags.IsKnown;

        var bow = this.weaponFactory.CreateWeapon(WeaponType.Bow);
        bow.AdditionalHit = 1;
        bow.Flags |= WeaponFlags.IsKnown;

        var arrows = this.weaponFactory.CreateWeapon(WeaponType.Arrow);
        arrows.Count = (byte)(random.Next(16) + 25);
        arrows.Flags |= WeaponFlags.IsKnown;

        var armor = this.armorFactory.CreateArmor(ArmorType.RingMail);
        armor.AmorClass -= 1;
        
        return new (new ExperienceCalculator<T>())
        {
            Name = "atogeib",
            Strength = 16,
            Experience = 0,
            ExperienceLevel = 1,
            AmorClass = 10,
            HitPoints = 12,
            MaxHitPoints = 12,
            ActiveWeapon = mace,
            Weapons = new List<Weapon>() {mace, bow, arrows},
            ActiveArmor = armor,
            Armors = new List<Armor>() {armor},
            Damage = new List<DiceThrow>() { diceThrow}
        };
    }
}