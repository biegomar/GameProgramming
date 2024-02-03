using NearlyRogue.Core.Dices;

namespace NearlyRogue.Core.Weapons;

public class WeaponFactory 
{
    private readonly Random random = new Random();
    
    private readonly Dice D0 = new (DiceType.D0);
    private readonly Dice D1 = new (DiceType.D1);
    private readonly Dice D2 = new (DiceType.D2);
    private readonly Dice D3 = new (DiceType.D3);
    private readonly Dice D4 = new (DiceType.D4);
    private readonly Dice D5 = new (DiceType.D5);
    private readonly Dice D6 = new (DiceType.D6);
    private readonly Dice D8 = new (DiceType.D8);
    private readonly Dice D10 = new (DiceType.D10);
    private readonly Dice D12 = new (DiceType.D12);
        
    public Weapon CreateWeapon(WeaponType weaponType) 
    {
        return weaponType switch 
        {
            WeaponType.Mace => GetMace(),
            WeaponType.Sword => GetSword(),
            WeaponType.Bow => GetBow(),
            WeaponType.Arrow => GetArrow(),
            WeaponType.Dagger => GetDagger(),
            WeaponType.TwoSword => GetTwoSword(),
            WeaponType.Dart => GetDart(),
            WeaponType.Crossbow => GetCrossbow(),
            WeaponType.Bolt => GetBolt(),
            WeaponType.Spear => GetSpear(),
            WeaponType.Flame => GetFlame(),
            _ => throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null)
        };
    }

    private Weapon GetMace() 
    {
        DiceThrow diceThrow = new(2, D4);
        DiceThrow diceThrowHurl = new(1, D3);
        
        return new Weapon 
        {
            Type = WeaponType.Mace,
            LaunchedByType = null,
            AdditionalDamage = 0,
            AdditionalHit = 0,
            Count = 1,
            Damage = new List<DiceThrow>() {diceThrow},
            HurlDamage = new List<DiceThrow>() {diceThrowHurl}
        };
    }
        
    private Weapon GetSword() 
    {
        DiceThrow diceThrow = new(3, D4);
        DiceThrow diceThrowHurl = new(1, D2);
        
        return new Weapon 
        {
            Type = WeaponType.Sword,
            LaunchedByType = null,
            AdditionalDamage = 0,
            AdditionalHit = 0,
            Count = 1,
            Damage = new List<DiceThrow>() {diceThrow},
            HurlDamage = new List<DiceThrow>() {diceThrowHurl}
        };
    }
        
    private Weapon GetBow() 
    {
        DiceThrow diceThrow = new(1, D1);
        DiceThrow diceThrowHurl = new(1, D1);
        
        return new Weapon 
        {
            Type = WeaponType.Bow,
            AdditionalDamage = 0,
            AdditionalHit = 0,
            Count = 1,
            Damage = new List<DiceThrow>() {diceThrow},
            HurlDamage = new List<DiceThrow>() {diceThrowHurl}
        };
    }
        
    private Weapon GetArrow() 
    {
        DiceThrow diceThrow = new(1, D1);
        DiceThrow diceThrowHurl = new(2, D3);
        
        return new Weapon 
        {
            Type = WeaponType.Arrow, 
            LaunchedByType = WeaponType.Bow,
            Flags = WeaponFlags.IsMany | WeaponFlags.IsMissile,
            AdditionalDamage = 0,
            AdditionalHit = 0,
            Count = this.GetCount(),
            Damage = new List<DiceThrow>() {diceThrow},
            HurlDamage = new List<DiceThrow>() {diceThrowHurl}
        };
    }
        
    private Weapon GetDagger() 
    {
        DiceThrow diceThrow = new(1, D6);
        DiceThrow diceThrowHurl = new(1, D4);
        
        return new Weapon 
        {
            Type = WeaponType.Dagger,
            Flags = WeaponFlags.IsMissile,
            AdditionalDamage = 0,
            AdditionalHit = 0,
            Count = 1,
            Damage = new List<DiceThrow>() {diceThrow},
            HurlDamage = new List<DiceThrow>() {diceThrowHurl}
        };
    }
        
    private Weapon GetTwoSword() 
    {
        DiceThrow diceThrow = new(4, D4);
        DiceThrow diceThrowHurl = new(1, D2);
        
        return new Weapon 
        {
            Type = WeaponType.TwoSword, 
            AdditionalDamage = 0,
            AdditionalHit = 0,
            Count = 1,
            Damage = new List<DiceThrow>() {diceThrow},
            HurlDamage = new List<DiceThrow>() {diceThrowHurl}
        };
    }
        
    private Weapon GetDart() 
    {
        DiceThrow diceThrow = new(1, D1);
        DiceThrow diceThrowHurl = new(1, D3);
        
        return new Weapon 
        {
            Type = WeaponType.Dart, 
            Flags = WeaponFlags.IsMany | WeaponFlags.IsMissile,
            AdditionalDamage = 0,
            AdditionalHit = 0,
            Count = this.GetCount(),
            Damage = new List<DiceThrow>() {diceThrow},
            HurlDamage = new List<DiceThrow>() {diceThrowHurl}
        };
    }

    private Weapon GetCrossbow() 
    {
        DiceThrow diceThrow = new(1, D1);
        DiceThrow diceThrowHurl = new(1, D1);
        
        return new Weapon 
        {
            Type = WeaponType.Crossbow, 
            AdditionalDamage = 0,
            AdditionalHit = 0,
            Count = 1,
            Damage = new List<DiceThrow>() {diceThrow},
            HurlDamage = new List<DiceThrow>() {diceThrowHurl}
        };
    }

    private Weapon GetBolt() 
    {
        DiceThrow diceThrow = new(1, D2);
        DiceThrow diceThrowHurl = new(2, D5);
        
        return new Weapon 
        {
            Type = WeaponType.Bolt, 
            LaunchedByType = WeaponType.Crossbow,
            Flags = WeaponFlags.IsMany | WeaponFlags.IsMissile,
            AdditionalDamage = 0,
            AdditionalHit = 0,
            Count = this.GetCount(),
            Damage = new List<DiceThrow>() {diceThrow},
            HurlDamage = new List<DiceThrow>() {diceThrowHurl}
        };
    }

    private Weapon GetSpear() 
    {
        DiceThrow diceThrow = new(2, D3);
        DiceThrow diceThrowHurl = new(1, D6);
        
        return new Weapon 
        {
            Type = WeaponType.Spear,
            Flags = WeaponFlags.IsMissile,
            AdditionalDamage = 0,
            AdditionalHit = 0,
            Count = 1,
            Damage = new List<DiceThrow>() {diceThrow},
            HurlDamage = new List<DiceThrow>() {diceThrowHurl}
        };
    }

    private Weapon GetFlame() 
    {
        DiceThrow diceThrow = new(2, D4);
        DiceThrow diceThrowHurl = new(1, D3);
        
        return new Weapon 
        {
            Type = WeaponType.Flame,
            AdditionalDamage = 0,
            AdditionalHit = 0,
            Count = 1,
            Damage = new List<DiceThrow>() {diceThrow},
            HurlDamage = new List<DiceThrow>() {diceThrowHurl}
        };
    }

    private byte GetCount()
    {
        return (byte)(this.random.Next(1, 9) + 8);
    }

}