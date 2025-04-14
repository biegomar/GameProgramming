using CellularAutomata.Cells;

namespace CellularAutomata.Interfaces;

public interface IPlayGround
{
    Vector Dimension { get; }
    
    CellBrightness this[Vector position] { get; set; }
    
    Cell GetCell(Vector position);
    
    void SetCell(Vector position, Cell cell);
}