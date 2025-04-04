namespace CellularAutomata;

public sealed class PlayGround: IPlayGround
{
    public Dictionary<Vector, Cell> Cells { get; init; }
    private Dictionary<Vector, Cell?> MovedCells;

    public Vector Dimension { get; init; }
    
    public PlayGround(Vector dimension)
    {
        Dimension = dimension;
        
        var capacity = dimension.X * dimension.Y;
        Cells = new Dictionary<Vector, Cell>(capacity);
        MovedCells = new Dictionary<Vector, Cell?>(capacity);
        
        Initialize();
    }

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

    public void SetCellToMoved(Vector position)
    {
        UpdateCellMovementStatus(position, true);
    }

    public void ResetMovedCells()
    {
        MovedCells = new Dictionary<Vector, Cell?>(Dimension.X * Dimension.Y);
    }

    public bool HasMoved(Vector position)
    {
        return Cells[position].HasMoved;
    }
    
    private void UpdateCellMovementStatus(Vector position, bool hasMoved = true)
    {
        var cell = Cells[position];
        cell.HasMoved = hasMoved;
        Cells[position] = cell;
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