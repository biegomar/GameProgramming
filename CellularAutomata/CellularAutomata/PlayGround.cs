namespace CellularAutomata;

public sealed class PlayGround: IPlayGround
{
    public Dictionary<Vector, Cell> Cells { get; init; }

    public PlayGround(Vector dimension)
    {
        Dimension = dimension;
        
        var capacity = dimension.X * dimension.Y;
        Cells = new Dictionary<Vector, Cell>(capacity);
        
        Initialize();
    }

    public Vector Dimension { get; init; }

    public CellState this[Vector position]
    {
        get => Cells[position].State;
        set
        {
            var cell = Cells[position];
            cell.State = value;
            Cells[position] = cell;
        }
    }

    private void Initialize()
    {
        const CellState defaultState = CellState.Empty;

        for (var x = 0; x < this.Dimension.X; x++)
        {
            for (var y = 0; y < this.Dimension.Y; y++)
            {
                this.Cells.Add(new Vector(x,y), new Cell(defaultState));
            }
        }
    }
}