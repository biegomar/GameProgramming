using NearlyRogue.Core.Dices;

namespace NearlyRogue.Core.Weapons;

public class Weapon
{
    public WeaponType Type { get; set; }
    public WeaponType? LaunchedByType { get; set; }
    public WeaponFlags Flags { get; set; }
    public byte AdditionalDamage { get; set; }
    public byte AdditionalHit { get; set; }
    
    public required IList<DiceThrow> Damage { get; set; }
    public required IList<DiceThrow> HurlDamage { get; set; }
}