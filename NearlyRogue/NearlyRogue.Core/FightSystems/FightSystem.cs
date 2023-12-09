namespace NearlyRogue.Core.FightSystems;

public class FightSystem
{
    private readonly Random random;
    
    
    public void BattleOneRound(ICreature attacker, ICreature defender)
    {
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
    
    private bool DoesSwingHit(ICreature attacker, ICreature defender, byte attackerHitBonus)
    {
        var res = this.random.Next(1,21);
        var need = 20 - attacker.ExperienceLevel - defender.AmorClass;

        return res + attackerHitBonus >= need;
    }
}