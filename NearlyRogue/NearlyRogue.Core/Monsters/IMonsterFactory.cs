namespace NearlyRogue.Core.Monsters;

public interface IMonsterFactory
{
    Monster CreateMonster(MonsterRace monsterRace);
}