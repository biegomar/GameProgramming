using NearlyRogue.Core.Numerics;

namespace NearlyRogue.Core.Movement;

public interface IMovement<in T>
{
    Vector ActualPosition { get; set; }
    void MoveTo(Vector position);
}