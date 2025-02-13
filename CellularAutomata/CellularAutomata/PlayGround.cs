namespace CellularAutomata;

public class PlayGround<T> : IPlayGround<T>
{
    public Dictionary<Vector, Cell<T>> Cells { get; init; } = new();

    public PlayGround(Vector dimension)
    {
        Dimension = dimension;
        Initialize();
    }

    public Vector Dimension { get; init; }
    
    public void ForEachCell(Action<Vector> action)
    {
        for (var x = 0; x < this.Dimension.X; x++)
        {
            for (var y = 0; y < this.Dimension.Y; y++)
            {
                for (var z = 0; z <= this.Dimension.Z; z++)
                {
                    action(new Vector(x, y, z));
                    
                    if (this.Dimension.Z == 0) break;
                }
            }
        }
    }

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

    public T this[(int x, int y, int z) position]
    {
        get => Cells[new Vector(position.x, position.y, position.z)].State;
        set => Cells[new Vector(position.x, position.y, position.z)].State = value;
    }


    private void Initialize()
    {
        for (var x = 0; x < this.Dimension.X; x++)
        {
            for (var y = 0; y < this.Dimension.Y; y++)
            {
                for (var z = 0; z <= this.Dimension.Z; z++)
                {
                    this.Cells.Add(new Vector(x,y,z), new Cell<T>(default!, (x, y, z)));

                    if (this.Dimension.Z == 0) break;
                }
            }
        }
    }
}