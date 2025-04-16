using CellularAutomata.Cells;

namespace CellularAutomata.Interfaces;

public interface IPlayGround
{
    Vector Dimension { get; }
    
    Cell GetCell(Vector position);
    
    void SetCell(Vector position, Cell cell);
    
    CellType GetCellType(Vector position);
    
    void SetCellType(Vector position, CellType cellType);
}