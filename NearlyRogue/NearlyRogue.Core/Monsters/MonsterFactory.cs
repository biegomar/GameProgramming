using NearlyRogue.Core.Dices;
using NearlyRogue.Core.FightSystem;

namespace NearlyRogue.Core.Monsters;

public class MonsterFactory
{
    private readonly IAttackStrategy standardAttackStrategy;
    private readonly IDefendStrategy standardDefendStrategy;
    private readonly Dice D0;
    private readonly Dice D2;
    private readonly Dice D3;
    private readonly Dice D4;
    private readonly Dice D5;
    private readonly Dice D6;
    private readonly Dice D8;
    private readonly Dice D10;
    private readonly Dice D12;
    
    public MonsterFactory(IAttackStrategy standardAttackStrategy, IDefendStrategy standardDefendStrategy)
    {
        this.standardAttackStrategy = standardAttackStrategy;
        this.standardDefendStrategy = standardDefendStrategy;
        this.D0 = new () {TypeOfDice = DiceType.D0};
        this.D2 = new () {TypeOfDice = DiceType.D2};
        this.D3 = new () {TypeOfDice = DiceType.D3};
        this.D4 = new () {TypeOfDice = DiceType.D4};
        this.D5 = new () {TypeOfDice = DiceType.D5};
        this.D6 = new () {TypeOfDice = DiceType.D6};
        this.D8 = new () {TypeOfDice = DiceType.D8};
        this.D10 = new () {TypeOfDice = DiceType.D10};
        this.D12 = new () {TypeOfDice = DiceType.D12};
    }
    
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
            MonsterRace.BlackUnicorn => GetBlackUnicorn(),
            MonsterRace.Vampire => GetVampire(),
            MonsterRace.Wraith => GetWraith(),
            MonsterRace.Xeroc => GetXeroc(),
            MonsterRace.Yeti => GetYeti(),
            MonsterRace.Zombie => GetZombie(),
            _ => throw new ArgumentOutOfRangeException(nameof(monsterRace), monsterRace, null)
        };
    }

    private static Monster GetZombie()
    {
        return new Monster(null, null);
    }

    private static Monster GetYeti()
    {
        return new Monster(null, null);
    }

    private static Monster GetXeroc()
    {
        return new Monster(null, null);
    }

    private static Monster GetWraith()
    {
        return new Monster(null, null);
    }

    private static Monster GetVampire()
    {
        return new Monster(null, null);
    }

    private static Monster GetBlackUnicorn()
    {
        return new Monster(null, null);
    }

    private static Monster GetTroll()
    {
        return new Monster(null, null);
    }

    private static Monster GetSnake()
    {
        return new Monster(null, null);
    }

    private static Monster GetRattlesnake()
    {
        return new Monster(null, null);
    }

    private static Monster GetQuagga()
    {
        return new Monster(null, null);
    }

    private static Monster GetPhantom()
    {
        return new Monster(null, null);
    }

    private static Monster GetOrc()
    {
        return new Monster(null, null);
    }

    private static Monster GetNymph()
    {
        return new Monster(null, null);
    }

    private static Monster GetMedusa()
    {
        return new Monster(null, null);
    }

    private static Monster GetLeprechaun()
    {
        return new Monster(null, null);
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

    private static Monster GetJabberwock()
    {
        return new Monster(null, null);
    }

    private static Monster GetIceMonster()
    {
        return new Monster(null, null);
    }

    private static Monster GetHobgoblin()
    {
        return new Monster(null, null);
    }

    private static Monster GetGriffin()
    {
        return new Monster(null, null);
    }

    private static Monster GetVenusFlytrap()
    {
        return new Monster(null, null);
    }

    private static Monster GetEmu()
    {
        return new Monster(null, null);
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