using NearlyRogue.Core.Monsters;

namespace NearlyRogue.Core.Players;

public class ExperienceCalculator : IExperienceCalculator
{
    public ushort GainExperience(Monster monster)
    {
        return 1;
    }
}