namespace CellularAutomata;

public interface IPlayGround
{
    Vector Dimension { get; }
    
    CellState this[Vector position] { get; set; }

    void SetCellToMoved(Vector position);
    
    void ResetMovedCells();

    bool HasMoved(Vector position);
}