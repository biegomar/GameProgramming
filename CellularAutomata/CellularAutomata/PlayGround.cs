namespace CellularAutomata;

public class PlayGround<T>
{
    public Dictionary<Vector, Cell<T>> cells = new();

    public PlayGround(Vector dimension)
    {
        Dimension = dimension;
        Initialize();
    }

    public Vector Dimension { get; init; }
    
    public T this[Vector position] { get => this.cells[position].State; set => this.cells[position].State = value; }

    private void Initialize()
    {
        for (var x = 0; x < this.Dimension.X; x++)
        {
            for (var y = 0; y < this.Dimension.Y; y++)
            {
                this.cells.Add(new Vector(x,y,0), new Cell<T>(default!));
            }
        }
    }
}