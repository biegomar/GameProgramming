using NearlyRogue.Core.Monsters;

namespace NearlyRogue.Core.Players;

public interface IExperienceCalculator<T>
{
    ushort GainExperience(Monster<T> monster);
}