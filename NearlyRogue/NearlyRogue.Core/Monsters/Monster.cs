using NearlyRogue.Core.Dices;
using NearlyRogue.Core.FightSystems;

namespace NearlyRogue.Core.Monsters;

public class Monster: IAttacker, IDefender
{
    public MonsterRace Race { get; init; }
    public string Name => Race.ToString();
    public byte TreasurePercentage { get; init; }
    public MonsterFlags Flags { get; init; }
    public ushort Experience { get; set; }
    public sbyte ExperienceLevel { get; set; }
    public ushort HitPoints { get; set; }
    public sbyte AmorClass { get; set; }
    public IList<DiceThrow> Damage { get; set; }
    public ushort Range { get; set; }
    
    private readonly IAttackStrategy attackStrategy;
    private readonly IDefendStrategy defendStrategy;

    public Monster(IAttackStrategy attackStrategy, IDefendStrategy defendStrategy)
    {
        this.attackStrategy = attackStrategy;
        this.defendStrategy = defendStrategy;
    }
    
    public void Attack(IDefender villain)
    {
        throw new NotImplementedException();
    }

    public void Defeat(IAttacker attacker)
    {
        throw new NotImplementedException();
    }
}