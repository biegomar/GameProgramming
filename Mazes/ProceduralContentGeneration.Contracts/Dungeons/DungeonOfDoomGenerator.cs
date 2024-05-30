using System;
using System.Collections.Generic;
using System.Linq;
using Mazes.Contracts.Cells;

namespace Mazes.Contracts.Dungeons
{
    public class DungeonOfDoomGenerator<T> : BaseDungeonGenerator<T>
    {
        private IDictionary<int, IList<Cell<T>>> rooms;
        
        private DungeonProperties properties;

        private Random random;

        public DungeonOfDoomGenerator(DungeonProperties properties)
        {
            this.properties = properties;
            this.random = new Random();
            this.rooms = new Dictionary<int, IList<Cell<T>>>();
        }
        
        public override IList<Cell<T>> Generate(IList<Cell<T>> cells)
        {
            int roomIndex = 0;
            var roomXDimension = (this.properties.PlaygroundDimension.X / this.properties.GridDimension.X) - 1;
            var roomYDimension = (this.properties.PlaygroundDimension.Y / this.properties.GridDimension.Y) - 1;

            var visibleRoomMatrix = this.GetVisibleRoomMatrix();
            
            for (int x = 0; x < this.properties.GridDimension.X; x++)
            {
                for (int y = 0; y < this.properties.GridDimension.Y; y++)
                {
                    if (visibleRoomMatrix[x, y])
                    {
                        var room = GenerateRoom(cells, new CellVector(x, y, 0), roomXDimension, roomYDimension);
                        this.rooms.Add(roomIndex, room);
                        roomIndex++;
                    }
                }
            }
            
            return cells;
        }

        private IList<Cell<T>> GenerateRoom(IList<Cell<T>> cells, CellVector sector, int maxRoomXDimension, int maxRoomYDimension)
        {
            IList<Cell<T>> result = new List<Cell<T>>();
            
            int width = 0;
            int height = 0;
            
            width = this.random.Next(1, maxRoomXDimension + 1);
            height = this.random.Next(1, maxRoomYDimension + 1);

            var correctorX = (maxRoomXDimension - width) / 2 + 1;
            var correctorY = (maxRoomYDimension - height) / 2 + 1;
            
            var roomStartPositionX = sector.X * maxRoomXDimension + sector.X + correctorX;
            var roomStartPositionY = sector.Y * maxRoomYDimension + sector.Y + correctorY;

            for (int x = roomStartPositionX; x < width + roomStartPositionX; x++)
            {
                for (int y = roomStartPositionY; y < height + roomStartPositionY; y++)
                {
                    var cell = GetCellByColumnAndRow(cells, x, y);
                    cell.IsVisible = true;
                    result.Add(cell);
                }
            }

            return result;
        }

        private bool[,] GetVisibleRoomMatrix()
        {
            var result = new bool[this.properties.GridDimension.X, this.properties.GridDimension.Y];
            
            SetAllCellsVisible(result);
            
            var numberOfGoneRooms = this.random.Next(this.properties.MaxGoneRooms + 1);
            var numberOfMarkedGoneRooms = 0;

            while (numberOfMarkedGoneRooms < numberOfGoneRooms)
            {
                var x = this.random.Next(this.properties.GridDimension.X);
                var y = this.random.Next(this.properties.GridDimension.Y);
                if (result[x, y])
                {
                    result[x, y] = false;
                    numberOfMarkedGoneRooms++;
                }
            }

            return result;
        }

        private void SetAllCellsVisible(bool[,] result)
        {
            for (int i = 0; i < this.properties.GridDimension.X; i++)
            {
                for (int j = 0; j < this.properties.GridDimension.Y; j++)
                {
                    result[i, j] = true;
                }
            }
        }
    }
}