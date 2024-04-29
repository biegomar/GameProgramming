using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mazes.Contracts
{
    public class Cell<T>
    {
        private readonly IDictionary<Directions, Cell<T>?> Neighbours = new Dictionary<Directions, Cell<T>?>();
        private readonly IList<Cell<T>> linkedCells = new List<Cell<T>>();
        
        public int X { get; }
        public int Y { get; }

        public T Item { get; set; }
        
        public Cell<T>? NorthernNeighbour
        {
            get => Neighbours[Directions.North];
            set => Neighbours[Directions.North] = value;
        }

        public Cell<T>? EasternNeighbour
        {
            get => Neighbours[Directions.East];
            set => Neighbours[Directions.East] = value;
        }

        public Cell<T>? SouthernNeighbour
        {
            get => Neighbours[Directions.South];
            set => Neighbours[Directions.South] = value;
        }

        public Cell<T>? WesternNeighbour
        {
            get => Neighbours[Directions.West];
            set => Neighbours[Directions.West] = value;
        }

        public IList<Cell<T>> LinkedCells => linkedCells;
        
        public Cell(int x = 0, int y = 0, Cell<T>? northernNeighbour = null, Cell<T>? easternNeighbour = null, Cell<T>? southernNeighbour = null, Cell<T>? westernNeighbour = null)
        {
            this.X = x;
            this.Y = y;
            Neighbours.Add(Directions.North, northernNeighbour);
            Neighbours.Add(Directions.East, easternNeighbour);
            Neighbours.Add(Directions.South, southernNeighbour);
            Neighbours.Add(Directions.West, westernNeighbour);
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
