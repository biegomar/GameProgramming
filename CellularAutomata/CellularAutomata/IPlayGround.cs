namespace CellularAutomata;

public interface IPlayGround
{
    Vector Dimension { get; }
    
    CellState this[Vector position] { get; set; }

    void SetCellToMoved(Vector position);
    
    void ClearCellToNotMoved(Vector position);

    bool HasMoved(Vector position);
}