namespace CellularAutomata;

public class PlayGroundArray<T> : IPlayGround<T>
{
    public Cell<T>[,] Cells { get; init; }
    
    public Vector Dimension { get; init; }

    public PlayGroundArray(Vector dimension)
    {
        Dimension = dimension;
        Cells = new Cell<T>[dimension.X, dimension.Y];
        
        Initialize();
    }
    
    public T this[Vector position]
    {
        get => Cells[position.X, position.Y].State;
        set => Cells[position.X, position.Y].State = value;
    }

    public T this[(int x, int y) position]
    {
        get => Cells[position.x, position.y].State;
        set => Cells[position.x, position.y].State = value;
    }
    
    private void Initialize()
    {
        var defaultState = default(T)!;
        
        for (var x = 0; x < this.Dimension.X; x++)
        {
            for (var y = 0; y < this.Dimension.Y; y++)
            {
                this.Cells[x, y] = new Cell<T>(defaultState, x, y);
            }
        }
    }
}