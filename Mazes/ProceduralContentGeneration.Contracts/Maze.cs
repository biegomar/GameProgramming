namespace Mazes.Contracts
{
    public class Maze<T>
    {
        public int Width { get; }
        public int Height { get; }
        
        public Cell<T>?[,] Cells { get; }

        public string Title { get; }

        private readonly IProceduralContentGenerator<T> _proceduralContentGenerator;
        private readonly IContentPrinter<T> mazePrinter;

        public Maze(MazeVector dimension, IProceduralContentGenerator<T> proceduralContentGenerator, IContentPrinter<T> contentPrinter) : this(dimension,
            proceduralContentGenerator, contentPrinter, string.Empty)
        {
        }

        public Maze(MazeVector dimension, IProceduralContentGenerator<T> proceduralContentGenerator, IContentPrinter<T> contentPrinter, string title)
        {
            this._proceduralContentGenerator = proceduralContentGenerator;
            this.mazePrinter = contentPrinter;
            this.Title = title;

            this.Cells = new Cell<T>[dimension.X, dimension.Y];
            this.Width = Cells.GetLength(0);
            this.Height = Cells.GetLength(1);
            
            InitializeMaze();
            LinkCellsInMaze();

            this.Cells = this._proceduralContentGenerator.Generate(this.Cells);
        }

        public void Draw(MazeVector startMazeVector)
        {
            this.mazePrinter.DrawMaze(this, startMazeVector);
        }

        public void DrawCellItems()
        {
            this.mazePrinter.DrawCellItems(this);
        }

        public void DrawItemAtPosition(MazeVector position, T item)
        {
            this.mazePrinter.DrawItemAtPosition(this, position, item);
        }

        private void InitializeMaze()
        {
            for (int column = 0; column < Cells.GetLength(0); column++)           
            {
                for(int row = 0; row < Cells.GetLength(1); row++)
                {
                    this.Cells[column, row] = new Cell<T>(column, row);
                }
            }
        }

        private void LinkCellsInMaze()
        {            
            for (int column = 0; column < Width; column++)
            {
                for (int row = 0; row < Height; row++)
                {
                    this.Cells[column, row].NorthernNeighbour = row - 1 < 0 ? null : this.Cells[column, row - 1];
                    this.Cells[column, row].EasternNeighbour = column + 1 >= Width ? null : this.Cells[column + 1, row];
                    this.Cells[column, row].SouthernNeighbour = row + 1 >= Height ? null : this.Cells[column, row + 1];
                    this.Cells[column, row].WesternNeighbour = column - 1 < 0 ? null : this.Cells[column - 1, row];
                }
            }
        }

        public void SetCellItem(CellItem<T> cellItem)
        {
            this.Cells[cellItem.Position.X, cellItem.Position.Y]!.Item = cellItem.Item;
        }

        public void ClearCellItem(MazeVector position)
        {
            this.Cells[position.X, position.Y]!.Item = default!;
        }
    }
}
