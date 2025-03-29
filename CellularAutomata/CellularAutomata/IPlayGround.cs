namespace CellularAutomata;

public interface IPlayGround
{
    Vector Dimension { get; }
    
    CellState this[Vector position] { get; set; }
}