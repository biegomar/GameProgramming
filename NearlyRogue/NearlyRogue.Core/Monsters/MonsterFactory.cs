using NearlyRogue.Core.Dices;
using NearlyRogue.Core.FightSystems;
using NearlyRogue.Core.FightSystems.StandardImpl;

namespace NearlyRogue.Core.Monsters;

public class MonsterFactory
{
    private readonly IAttackStrategy standardAttackStrategy = new MonsterAttackStrategy();
    private readonly IDefendStrategy standardDefendStrategy = new MonsterDefendStrategy();
    private readonly Dice D0 = new (DiceType.D0);
    private readonly Dice D2 = new (DiceType.D2);
    private readonly Dice D3 = new (DiceType.D3);
    private readonly Dice D4 = new (DiceType.D4);
    private readonly Dice D5 = new (DiceType.D5);
    private readonly Dice D6 = new (DiceType.D6);
    private readonly Dice D8 = new (DiceType.D8);
    private readonly Dice D10 = new (DiceType.D10);
    private readonly Dice D12 = new (DiceType.D12);

    public Monster CreateMonster(MonsterRace monsterRace)
    {
        return monsterRace switch
        {
            MonsterRace.Aquator => GetAquator(),
            MonsterRace.Bat => GetBat(),
            MonsterRace.Centaur => GetCentaur(),
            MonsterRace.Dragon => GetDragon(),
            MonsterRace.Emu => GetEmu(),
            MonsterRace.VenusFlytrap => GetVenusFlytrap(),
            MonsterRace.Griffin => GetGriffin(),
            MonsterRace.Hobgoblin => GetHobgoblin(),
            MonsterRace.IceMonster => GetIceMonster(),
            MonsterRace.Jabberwock => GetJabberwock(),
            MonsterRace.Kestrel => GetKestrel(),
            MonsterRace.Leprechaun => GetLeprechaun(),
            MonsterRace.Medusa => GetMedusa(),
            MonsterRace.Nymph => GetNymph(),
            MonsterRace.Orc => GetOrc(),
            MonsterRace.Phantom => GetPhantom(),
            MonsterRace.Quagga => GetQuagga(),
            MonsterRace.Rattlesnake => GetRattlesnake(),
            MonsterRace.Snake => GetSnake(),
            MonsterRace.Troll => GetTroll(),
            MonsterRace.Urvile => GetUrvile(),
            MonsterRace.Vampire => GetVampire(),
            MonsterRace.Wraith => GetWraith(),
            MonsterRace.Xeroc => GetXeroc(),
            MonsterRace.Yeti => GetYeti(),
            MonsterRace.Zombie => GetZombie(),
            _ => throw new ArgumentOutOfRangeException(nameof(monsterRace), monsterRace, null)
        };
    }

    private Monster GetZombie()
    {
        sbyte expLevel = 2;
        DiceThrow diceThrow = new(1, D8);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Zombie,
            ExperienceLevel = expLevel,
            Experience = 6,
            Flags = MonsterFlags.Mean,
            TreasurePercentage = 0,
            AmorClass = 8,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetYeti()
    {
        sbyte expLevel = 4;
        DiceThrow diceThrow = new(1, D6);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Yeti,
            ExperienceLevel = expLevel,
            Experience = 50,
            TreasurePercentage = 30,
            AmorClass = 6,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow, diceThrow}
        };
    }

    private Monster GetXeroc()
    {
        sbyte expLevel = 7;
        DiceThrow diceThrow = new(3, D4);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Xeroc,
            ExperienceLevel = expLevel,
            Experience = 100,
            TreasurePercentage = 30,
            AmorClass = 7,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetWraith()
    {
        sbyte expLevel = 5;
        DiceThrow diceThrow = new(1, D6);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Wraith,
            ExperienceLevel = expLevel,
            Experience = 55,
            TreasurePercentage = 0,
            AmorClass = 4,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetVampire()
    {
        sbyte expLevel = 8;
        DiceThrow diceThrow = new(1, D10);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Vampire,
            ExperienceLevel = expLevel,
            Experience = 350,
            Flags = MonsterFlags.Mean | MonsterFlags.Regeneration,
            TreasurePercentage = 20,
            AmorClass = 1,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetUrvile()
    {
        sbyte expLevel = 7;
        DiceThrow diceThrowD3 = new(1, D3);
        DiceThrow diceThrowD6 = new(4, D6);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Urvile,
            ExperienceLevel = expLevel,
            Experience = 190,
            Flags = MonsterFlags.Mean,
            TreasurePercentage = 0,
            AmorClass = -2,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrowD3, diceThrowD3, diceThrowD3, diceThrowD6}
        };
    }

    private Monster GetTroll()
    {
        sbyte expLevel = 6;
        DiceThrow diceThrowD6 = new(2, D6);
        DiceThrow diceThrowD8 = new(1, D8);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Troll,
            ExperienceLevel = expLevel,
            Experience = 120,
            Flags = MonsterFlags.Mean | MonsterFlags.Regeneration,
            TreasurePercentage = 50,
            AmorClass = 4,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrowD8, diceThrowD8, diceThrowD6}
        };
    }

    private Monster GetSnake()
    {
        sbyte expLevel = 2;
        DiceThrow diceThrow = new(1, D3);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Snake,
            ExperienceLevel = expLevel,
            Experience = 1,
            Flags = MonsterFlags.Mean,
            TreasurePercentage = 0,
            AmorClass = 8,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetRattlesnake()
    {
        sbyte expLevel = 2;
        DiceThrow diceThrow = new(1, D6);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Rattlesnake,
            ExperienceLevel = expLevel,
            Experience = 9,
            Flags = MonsterFlags.Mean,
            TreasurePercentage = 0,
            AmorClass = 3,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetQuagga()
    {
        sbyte expLevel = 3;
        DiceThrow diceThrowD2 = new(1, D2);
        DiceThrow diceThrowD4 = new(1, D4);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Quagga,
            ExperienceLevel = expLevel,
            Experience = 32,
            Flags = MonsterFlags.Mean,
            TreasurePercentage = 30,
            AmorClass = 2,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrowD2, diceThrowD2, diceThrowD4}
        };
    }

    private Monster GetPhantom()
    {
        sbyte expLevel = 8;
        DiceThrow diceThrow = new(4, D4);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Phantom,
            ExperienceLevel = expLevel,
            Experience = 120,
            Flags = MonsterFlags.Invisible,
            TreasurePercentage = 0,
            AmorClass = 3,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetOrc()
    {
        sbyte expLevel = 1;
        DiceThrow diceThrow = new(1, D8);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Orc,
            ExperienceLevel = expLevel,
            Experience = 5,
            Flags = MonsterFlags.Greedy,
            TreasurePercentage = 15,
            AmorClass = 6,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetNymph()
    {
        sbyte expLevel = 3;
        DiceThrow diceThrow = new(0, D0);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Nymph,
            ExperienceLevel = expLevel,
            Experience = 37,
            TreasurePercentage = 100,
            AmorClass = 9,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetMedusa()
    {
        sbyte expLevel = 8;
        DiceThrow diceThrowD4 = new(3, D4);
        DiceThrow diceThrowD5 = new(2, D5);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Medusa,
            ExperienceLevel = expLevel,
            Experience = 200,
            Flags = MonsterFlags.Mean,
            TreasurePercentage = 40,
            AmorClass = 2,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrowD4, diceThrowD4, diceThrowD5}
        };
    }

    private Monster GetLeprechaun()
    {
        sbyte expLevel = 3;
        DiceThrow diceThrow = new(1, D2);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Leprechaun,
            ExperienceLevel = expLevel,
            Experience = 10,
            Flags = MonsterFlags.Greedy,
            TreasurePercentage = 0,
            AmorClass = 8,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetKestrel()
    {
        sbyte expLevel = 1;
        DiceThrow diceThrow = new(1, D4);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Kestrel,
            ExperienceLevel = expLevel,
            Experience = 1,
            Flags = MonsterFlags.Mean | MonsterFlags.Flying,
            TreasurePercentage = 0,
            AmorClass = 7,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetJabberwock()
    {
        sbyte expLevel = 15;
        DiceThrow diceThrowD4 = new(2, D4);
        DiceThrow diceThrowD12 = new(2, D12);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Jabberwock,
            ExperienceLevel = expLevel,
            Experience = 4000,
            TreasurePercentage = 70,
            AmorClass = 6,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrowD12, diceThrowD4}
        };
    }

    private Monster GetIceMonster()
    {
        sbyte expLevel = 1;
        DiceThrow diceThrow = new(1, D2);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.IceMonster,
            ExperienceLevel = expLevel,
            Experience = 15,
            Flags = MonsterFlags.Mean,
            TreasurePercentage = 0,
            AmorClass = 9,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetHobgoblin()
    {
        sbyte expLevel = 1;
        DiceThrow diceThrow = new(1, D8);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Hobgoblin,
            ExperienceLevel = expLevel,
            Experience = 3,
            Flags = MonsterFlags.Mean,
            TreasurePercentage = 0,
            AmorClass = 5,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetGriffin()
    {
        sbyte expLevel = 13;
        DiceThrow diceThrowD3 = new(4, D3);
        DiceThrow diceThrowD5 = new(3, D5);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Griffin,
            ExperienceLevel = expLevel,
            Experience = 2000,
            Flags = MonsterFlags.Mean | MonsterFlags.Flying | MonsterFlags.Regeneration,
            TreasurePercentage = 20,
            AmorClass = 2,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrowD3, diceThrowD5, diceThrowD3}
        };
    }

    private Monster GetVenusFlytrap()
    {
        //special behaviour needed.
        
        sbyte expLevel = 8;
        DiceThrow diceThrow = new(1, D0);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.VenusFlytrap,
            ExperienceLevel = expLevel,
            Experience = 80,
            Flags = MonsterFlags.Mean,
            TreasurePercentage = 0,
            AmorClass = 3,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetEmu()
    {
        sbyte expLevel = 1;
        DiceThrow diceThrow = new(1, D2);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Emu,
            ExperienceLevel = expLevel,
            Experience = 2,
            Flags = MonsterFlags.Mean,
            TreasurePercentage = 0,
            AmorClass = 7,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetDragon()
    {
        sbyte expLevel = 10;
        DiceThrow diceThrowD8 = new(1, D8);
        DiceThrow diceThrowD10 = new(3, D10);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Dragon,
            ExperienceLevel = expLevel,
            Experience = 6800,
            Flags = MonsterFlags.Mean,
            TreasurePercentage = 100,
            AmorClass = -1,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrowD8, diceThrowD8, diceThrowD10}
        };
    }

    private Monster GetCentaur()
    {
        sbyte expLevel = 4;
        DiceThrow diceThrow = new(1, D6);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Centaur,
            ExperienceLevel = expLevel,
            Experience = 25,
            TreasurePercentage = 15,
            AmorClass = 4,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow, diceThrow}
        };
    }

    private Monster GetBat()
    {
        sbyte expLevel = 1;
        DiceThrow diceThrow = new(1, D2);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Bat,
            ExperienceLevel = expLevel,
            Experience = 1,
            Flags = MonsterFlags.Flying,
            TreasurePercentage = 0,
            AmorClass = 3,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow}
        };
    }

    private Monster GetAquator()
    {
        sbyte expLevel = 5;
        DiceThrow diceThrow = new(0, D0);
        
        return new(standardAttackStrategy, standardDefendStrategy)
        {
            Race = MonsterRace.Aquator,
            ExperienceLevel = expLevel,
            Experience = 20,
            Flags = MonsterFlags.Mean,
            TreasurePercentage = 0,
            AmorClass = 2,
            HitPoints = D8.Roll(expLevel),
            Damage = new List<DiceThrow>() {diceThrow, diceThrow}
        };
    }
}