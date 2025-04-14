using CellularAutomata.Cells;
using CellularAutomata.MaterialFlow;

namespace CellularAutomata.Interfaces;

public interface IRuleSet
{
    CellBrightness ApplyRules(IPlayGround playGround, Vector position);
    
    MaterialMovement? ApplyMaterialRules(IPlayGround playGround, Vector position);
    
    IPlayGround ApplySpawnRules(IPlayGround playGround, Vector spawnPosition, Vector brushSize, double probability);
}