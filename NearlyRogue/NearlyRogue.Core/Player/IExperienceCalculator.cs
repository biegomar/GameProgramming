using NearlyRogue.Core.Monsters;

namespace NearlyRogue.Core.Player;

public interface IExperienceCalculator
{
    ushort GainExperince(Monster monster);
}