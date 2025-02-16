namespace CellularAutomata;

public class PlayGroundArray<T> : IPlayGround<T>
{
    public Cell<T>[,,] Cells { get; init; }
    
    public Vector Dimension { get; init; }

    public PlayGroundArray(Vector dimension)
    {
        Dimension = dimension;
        if (dimension.Z == 0)
        {
            Cells = new Cell<T>[(int)dimension.X, (int)dimension.Y, 1];
        }
        else
        {
            Cells = new Cell<T>[(int)dimension.X, (int)dimension.Y, (int)dimension.Z];
        }
        
        Initialize();
    }
    
    public T this[Vector position]
    {
        get => Cells[(int)position.X, (int)position.Y, (int)position.Z].State;
        set => Cells[(int)position.X, (int)position.Y, (int)position.Z].State = value;
    }

    public T this[(int x, int y, int z) position]
    {
        get => Cells[position.x, position.y, position.z].State;
        set => Cells[position.x, position.y, position.z].State = value;
    }
    
    private void Initialize()
    {
        var defaultState = default(T)!;
        
        for (var x = 0; x < this.Dimension.X; x++)
        {
            for (var y = 0; y < this.Dimension.Y; y++)
            {
                for (var z = 0; z <= this.Dimension.Z; z++)
                {
                    this.Cells[x, y, z] = new Cell<T>(defaultState, (x, y, z));
                    
                    if (this.Dimension.Z == 0) break;
                } 
            }
        }
    }
}