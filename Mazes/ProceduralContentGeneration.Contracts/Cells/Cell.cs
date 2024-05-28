using System.Collections.Generic;

namespace Mazes.Contracts.Cells
{
    public class Cell<T>
    {
        private readonly IDictionary<Directions, Cell<T>?> neighbours = new Dictionary<Directions, Cell<T>?>();
        protected readonly IList<Cell<T>> linkedCells = new List<Cell<T>>();
        
        public int X { get; }
        public int Y { get; }

        public T Item { get; set; }

        public bool IsVisible { get; set; }
        
        public bool IsVisited { get; set; }

        public int PathCount { get; set; }

        public Cell<T> Predecessor { get; set; }
        
        public Cell<T>? NorthernNeighbour
        {
            get => neighbours[Directions.North];
            set => neighbours[Directions.North] = value;
        }

        public Cell<T>? EasternNeighbour
        {
            get => neighbours[Directions.East];
            set => neighbours[Directions.East] = value;
        }

        public Cell<T>? SouthernNeighbour
        {
            get => neighbours[Directions.South];
            set => neighbours[Directions.South] = value;
        }

        public Cell<T>? WesternNeighbour
        {
            get => neighbours[Directions.West];
            set => neighbours[Directions.West] = value;
        }

        public IList<Cell<T>> LinkedCells => this.linkedCells;

        public IDictionary<Directions, Cell<T>?> Neighbours => this.neighbours;
        
        public Cell(int x = 0, int y = 0, Cell<T>? northernNeighbour = null, Cell<T>? easternNeighbour = null, Cell<T>? southernNeighbour = null, Cell<T>? westernNeighbour = null)
        {
            this.X = x;
            this.Y = y;
            neighbours.Add(Directions.North, northernNeighbour);
            neighbours.Add(Directions.East, easternNeighbour);
            neighbours.Add(Directions.South, southernNeighbour);
            neighbours.Add(Directions.West, westernNeighbour);
        }

        public void LinkCell(Cell<T>? cellToLink)
        {
            if (!this.LinkedCells.Contains(cellToLink))
            {
                this.LinkedCells.Add(cellToLink);
                cellToLink.LinkCell(this);
            }
        }
    }
}
