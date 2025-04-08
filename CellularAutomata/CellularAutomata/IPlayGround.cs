namespace CellularAutomata;

public interface IPlayGround
{
    Vector Dimension { get; }
    
    CellState this[Vector position] { get; set; }

    void MarkAsProcessedRight(Vector position);
    void MarkAsProcessedLeft(Vector position);
    
    void ResetMovedCells();

    bool IsProcessedRight(Vector position);
    bool IsProcessedLeft(Vector position);
    
    Cell GetCell(Vector position);
}