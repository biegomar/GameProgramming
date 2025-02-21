namespace CellularAutomata;

public class PlayGround<T> : IPlayGround<T>
{
    public Dictionary<Vector, Cell<T>> Cells { get; init; }

    public PlayGround(Vector dimension)
    {
        Dimension = dimension;
        
        var capacity = dimension.X * dimension.Y;
        Cells = new Dictionary<Vector, Cell<T>>(capacity);
        
        Initialize();
    }

    public Vector Dimension { get; init; }
    
    public T this[Vector position]
    {
        get => Cells[position].State;
        set
        {
            var cell = Cells[position];
            cell.State = value;
            Cells[position] = cell;
        }

    }

    public T this[(int x, int y) position]
    {
        get => Cells[new Vector(position.x, position.y)].State;
        set
        {
            var positionVector = new Vector(position.x, position.y);
            var cell = Cells[positionVector];
            cell.State = value;
            Cells[positionVector] = cell;
        }

    }


    private void Initialize()
    {
        var defaultState = default(T)!;

        for (var x = 0; x < this.Dimension.X; x++)
        {
            for (var y = 0; y < this.Dimension.Y; y++)
            {
                this.Cells.Add(new Vector(x,y), new Cell<T>(defaultState, x, y));
            }
        }
    }
}