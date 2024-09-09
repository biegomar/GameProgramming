using System.Collections.Generic;

namespace Mazes.Contracts.Cells
{
    public class Room<T>
    {
        private readonly IDictionary<Directions, Room<T>?> neighbours = new Dictionary<Directions, Room<T>?>();
        protected readonly IList<Room<T>> linkedRooms = new List<Room<T>>();
        
        public int X { get; }
        public int Y { get; }
        
        public bool IsVisible { get; set; }
        
        public IList<Cell<T>> Cells { get; set; } = new List<Cell<T>>();
        public bool Connected { get; set; } = false;
        
        public Room<T>? NorthernNeighbour
        {
            get => neighbours[Directions.North];
            set => neighbours[Directions.North] = value;
        }

        public Room<T>? EasternNeighbour
        {
            get => neighbours[Directions.East];
            set => neighbours[Directions.East] = value;
        }

        public Room<T>? SouthernNeighbour
        {
            get => neighbours[Directions.South];
            set => neighbours[Directions.South] = value;
        }

        public Room<T>? WesternNeighbour
        {
            get => neighbours[Directions.West];
            set => neighbours[Directions.West] = value;
        }
        
        public IList<Room<T>> LinkedRooms => this.linkedRooms;
        
        public IDictionary<Directions, Room<T>?> Neighbours => this.neighbours;
        
        public Room(int x = 0, int y = 0, Room<T>? northernNeighbour = null, Room<T>? easternNeighbour = null, Room<T>? southernNeighbour = null, Room<T>? westernNeighbour = null)
        {
            this.X = x;
            this.Y = y;
            neighbours.Add(Directions.North, northernNeighbour);
            neighbours.Add(Directions.East, easternNeighbour);
            neighbours.Add(Directions.South, southernNeighbour);
            neighbours.Add(Directions.West, westernNeighbour);
        }
    }
}