using NearlyRogue.Core.Monsters;

namespace NearlyRogue.Core.Players;

public class ExperienceCalculator<T> : IExperienceCalculator<T>
{
    public ushort GainExperience(Monster<T> monster)
    {
        return 1;
    }
}