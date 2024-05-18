using Mazes.Contracts;
using NearlyRogue.Core.Amors;
using NearlyRogue.Core.Dices;
using NearlyRogue.Core.FightSystems;
using NearlyRogue.Core.Monsters;
using NearlyRogue.Core.Players;
using NearlyRogue.Core.Weapons;

namespace NearlyRogue.Cli;

public static class Preparations
{
    public static Player<char> CreatePlayer(Landscape<ICreature<char>> landscape)
    {
        Random random = new ();
        WeaponFactory weaponFactory = new();
        ArmorFactory armorFactory = new();
        
        DiceThrow diceThrow = new(1, new Dice(DiceType.D4));
        var mace = weaponFactory.CreateWeapon(WeaponType.Mace);
        mace.AdditionalHit = 1;
        mace.AdditionalDamage = 1;
        mace.Flags |= WeaponFlags.IsKnown;

        var bow = weaponFactory.CreateWeapon(WeaponType.Bow);
        bow.AdditionalHit = 1;
        bow.Flags |= WeaponFlags.IsKnown;

        var arrows = weaponFactory.CreateWeapon(WeaponType.Arrow);
        arrows.Count = (byte)(random.Next(16) + 25);
        arrows.Flags |= WeaponFlags.IsKnown;

        var armor = armorFactory.CreateArmor(ArmorType.RingMail);
        armor.AmorClass -= 1;
        
        return new (new ExperienceCalculator<char>())
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
            Icon = '@',
            Armors = new List<Armor>() {armor},
            Damage = new List<DiceThrow>() { diceThrow}
        };
    }

    public static IDictionary<MonsterRace, char> GetIcons()
    {
        IDictionary<MonsterRace, char> monsterDictionary = new Dictionary<MonsterRace, char>();

        foreach (MonsterRace monster in Enum.GetValues(typeof(MonsterRace)))
        {
            monsterDictionary.Add(monster, monster.ToString()[0]);
        }

        return monsterDictionary;
    }
}