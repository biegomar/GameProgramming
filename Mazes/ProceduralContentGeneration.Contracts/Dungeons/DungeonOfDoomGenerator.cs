using System;
using System.Collections.Generic;
using System.Linq;
using Mazes.Contracts.Cells;

namespace Mazes.Contracts.Dungeons
{
    public class DungeonOfDoomGenerator<T> : BaseDungeonGenerator<T>
    {
        private DungeonProperties properties;

        private Random random;

        public DungeonOfDoomGenerator(DungeonProperties properties)
        {
            this.properties = properties;
            this.random = new Random();
        }
        
        public override IList<Cell<T>> Generate(IList<Cell<T>> cells)
        {
            var roomXDimension = (this.properties.PlaygroundDimension.X / this.properties.GridDimension.X) - 2;
            var roomYDimension = (this.properties.PlaygroundDimension.Y / this.properties.GridDimension.Y) - 2;

            var visibleRoomMatrix = this.GetVisibleRoomMatrix();
            
            for (int x = 0; x < this.properties.GridDimension.X; x++)
            {
                for (int y = 0; y < this.properties.GridDimension.Y; y++)
                {
                    if (visibleRoomMatrix[x, y])
                    {
                        GenerateRoom(cells, new CellVector(x, y, 0), roomXDimension, roomYDimension);
                    }
                }
            }
            
            return cells;
        }

        private IList<Cell<T>> GenerateRoom(IList<Cell<T>> cells, CellVector sector, int maxRoomXDimension, int maxRoomYDimension)
        {
            var width = this.random.Next(maxRoomXDimension + 1);
            var height = this.random.Next(maxRoomYDimension + 1);

            var roomStartPositionX = sector.X * maxRoomXDimension;
            var roomStartPositionY = sector.Y * maxRoomYDimension;

            for (int x = roomStartPositionX; x < width + roomStartPositionX; x++)
            {
                for (int y = roomStartPositionY; y < height + roomStartPositionY; y++)
                {
                    var cell = GetCellByColumnAndRow(cells, x, y);
                    cell.IsVisible = true;
                }
            }

            return cells;
        }

        private bool[,] GetVisibleRoomMatrix()
        {
            var result = new bool[this.properties.GridDimension.X, this.properties.GridDimension.Y];
            for (int i = 0; i < this.properties.GridDimension.X; i++)
            {
                for (int j = 0; j < this.properties.GridDimension.Y; j++)
                {
                    result[i, j] = true;
                }
            }
            
            var numberOfGoneRooms = this.random.Next(this.properties.MaxGoneRooms + 1);
            var numberOfMarkedGoneRooms = 0;

            while (numberOfGoneRooms < numberOfMarkedGoneRooms)
            {
                var x = this.random.Next(this.properties.GridDimension.X);
                var y = this.random.Next(this.properties.GridDimension.Y);
                if (result[x, y] != false)
                {
                    result[x, y] = false;
                    numberOfGoneRooms++;
                }
            }

            return result;
        }
    }
}