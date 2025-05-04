using CellularAutomata.Cells;
using CellularAutomata.PlayGrounds;

namespace CellularAutomata.MaterialFlow.MaterialHandler;

public sealed class SnowHandler(Vector dimension, uint seed = 100) : BaseMaterialHandler(dimension, seed)
{
    public override MaterialMovement? ApplyRules(PlayGround playGround, Vector position, Cell cell)
    {
        throw new NotImplementedException();
    }
}