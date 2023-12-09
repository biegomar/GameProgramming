using NearlyRogue.Core.Monsters;

namespace NearlyRogue.Core.Players;

public interface IExperienceCalculator
{
    ushort GainExperience(Monster monster);
}