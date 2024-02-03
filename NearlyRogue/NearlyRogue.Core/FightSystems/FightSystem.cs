using NearlyRogue.Core.Dices;
using NearlyRogue.Core.Players;

namespace NearlyRogue.Core.FightSystems;

public class FightSystem
{
    private readonly Random random = new Random();
    private byte additionalDamage;
    private byte additionalHit;
    private IList<DiceThrow> damage = new List<DiceThrow>();
    
    public void BattleOneRound(ICreature attacker, ICreature defender)
    {
        if (attacker is Player player)
        {
            this.SetAdditionalDamageAndHit(player);
        }
        
        foreach (var diceThrow in attacker.Damage)
        {
            var strengthCorrector = (byte)(this.additionalHit + this.CalculateStrengthCorrector(attacker.Strength));
            if (DoesSwingHit(attacker, defender, strengthCorrector))
            {
                ushort rollResult = diceThrow.Dice.Roll(diceThrow.Tries);

                ushort attackResult = (ushort)(rollResult + this.additionalDamage + this.CalculateDamageCorrector(attacker.Strength));
                
                defender.HitPoints -= Math.Max((ushort)0, attackResult);
            }
        }
    }
    
    public sbyte CalculateStrengthCorrector(ushort strength)
    {
        sbyte add = 4;

        if (strength < 8) return (sbyte)(strength - 7);
        
        if (strength < 31) add--;
        if (strength < 21) add--;
        if (strength < 19) add--;
        if (strength < 17) add--;
        
        return add;
    }
    
    public sbyte CalculateDamageCorrector(ushort strength)
    {
        sbyte add = 6;

        if (strength < 8) return (sbyte)(strength - 7);
        
        if (strength < 31) add--;
        if (strength < 22) add--;
        if (strength < 20) add--;
        if (strength < 18) add--;
        if (strength < 17) add--;
        if (strength < 16) add--;
        
        return add;
    }
    
    private bool DoesSwingHit(ICreature attacker, ICreature defender, byte attackerHitBonus)
    {
        var res = this.random.Next(1,21);
        var need = 20 - attacker.ExperienceLevel - defender.AmorClass;

        return res + attackerHitBonus >= need;
    }

    private void SetAdditionalDamageAndHit(Player attacker)
    {
        if (attacker.ActiveWeapon != null)
        {
            this.additionalDamage = attacker.ActiveWeapon.AdditionalDamage;
            this.additionalHit = attacker.ActiveWeapon.AdditionalHit;
            this.damage = attacker.ActiveWeapon.Damage;
        }
    }
}