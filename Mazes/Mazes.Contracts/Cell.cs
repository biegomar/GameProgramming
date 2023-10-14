using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazes.Contracts
{
    public class Cell
    {
        private readonly IDictionary<Directions, Cell?> Neighbours = new Dictionary<Directions, Cell?>();

        public Cell? NothernNeighbour
        {
            get => Neighbours[Directions.North];
            set => Neighbours[Directions.North] = value;
        }

        public Cell? EasternNeighbour
        {
            get => Neighbours[Directions.East];
            set => Neighbours[Directions.East] = value;
        }

        public Cell? SouthernNeighbour
        {
            get => Neighbours[Directions.South];
            set => Neighbours[Directions.South] = value;
        }

        public Cell? WesternNeighbour
        {
            get => Neighbours[Directions.West];
            set => Neighbours[Directions.West] = value;
        }

        public Cell(Cell? northernNeighbour = null, Cell? easternNeighbour = null, Cell? southernNeighbour = null, Cell? westernNeighbout = null)
        {
            Neighbours.Add(Directions.North, northernNeighbour);
            Neighbours.Add(Directions.East, easternNeighbour);
            Neighbours.Add(Directions.South, southernNeighbour);
            Neighbours.Add(Directions.West, westernNeighbout);
        }
    }
}
