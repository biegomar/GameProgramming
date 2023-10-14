using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazes.Contracts
{
    internal class Cell
    {
        private readonly IDictionary<Directions, Cell?> Neighbours = new Dictionary<Directions, Cell?>();

        public Cell(Cell? northernNeighbour = null, Cell? easternNeighbour = null, Cell? southernNeighbour = null, Cell? westernNeighbout = null)
        {
            Neighbours.Add(Directions.North, northernNeighbour);
            Neighbours.Add(Directions.East, easternNeighbour);
            Neighbours.Add(Directions.South, southernNeighbour);
            Neighbours.Add(Directions.West, westernNeighbout);
        }
    }
}
