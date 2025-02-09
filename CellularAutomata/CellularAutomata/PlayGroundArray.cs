namespace CellularAutomata;

public class PlayGroundArray<T> : PlayGround<T>
{
    public Cell<T>[,,] Cells;

    public PlayGroundArray(Vector dimension) : base(dimension)
    {
        Dimension = dimension;
        Cells = new Cell<T>[(int)dimension.X + 1, (int)dimension.Y + 1, (int)dimension.Z + 1];
        Initialize();
    }

    public Vector Dimension { get; init; }
    
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
        for (var x = 0; x < this.Dimension.X + 1; x++)
        {
            for (var y = 0; y < this.Dimension.Y + 1; y++)
            {
                for (var z = 0; z < this.Dimension.Z + 1; z++)
                {
                    Cells[x, y, z] = new Cell<T>(default!); // Zelle initialisieren
                }
            }
        }

    }
}