namespace CellularAutomata;

public interface IPlayGround<T>
{
    Vector Dimension { get; }
    
    T this[Vector position] { get; set; }
    T this[(int x, int y, int z) position] { get; set; }

 
}