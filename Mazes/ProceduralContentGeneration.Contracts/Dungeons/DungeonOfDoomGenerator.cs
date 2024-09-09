using System;
using System.Collections.Generic;
using System.Linq;
using Mazes.Contracts.Cells;

namespace Mazes.Contracts.Dungeons
{
    public class DungeonOfDoomGenerator<T> : BaseDungeonGenerator<T>
    {
        private IList<Room<T>> rooms;
        private DungeonProperties properties;
        
        private Random random;

        public DungeonOfDoomGenerator(DungeonProperties properties)
        {
            this.properties = properties;
            this.random = new Random();
            this.rooms = new List<Room<T>>();
        }

        public override IList<Cell<T>> Generate(IList<Cell<T>> cells)
        {
            var roomXDimension = (this.properties.PlaygroundDimension.X / this.properties.GridDimension.X) - 1;
            var roomYDimension = (this.properties.PlaygroundDimension.Y / this.properties.GridDimension.Y) - 1;

            this.GenerateRooms(cells, roomXDimension, roomYDimension);

            this.LinkRooms();
            
            return cells;
        }

        private void GenerateRooms(IList<Cell<T>> cells, int roomXDimension, int roomYDimension)
        {
            var visibleRoomMatrix = this.GetVisibleRoomMatrix();

            for (var x = 0; x < this.properties.GridDimension.X; x++)
            {
                for (var y = 0; y < this.properties.GridDimension.Y; y++)
                {
                    if (visibleRoomMatrix[x, y])
                    {
                        var room = GenerateRoom(cells, new CellVector(x, y, 0), roomXDimension, roomYDimension);
                        this.rooms.Add(room);
                    }
                }
            }
        }

        private Room<T> GenerateRoom(IList<Cell<T>> cells, CellVector sector, int maxRoomXDimension, int maxRoomYDimension)
        {
            var result = new Room<T>(sector.X, sector.Y);
            
            var width = 0;
            var height = 0;
            
            width = this.random.Next(1, maxRoomXDimension + 1);
            height = this.random.Next(1, maxRoomYDimension + 1);

            var correctorX = (maxRoomXDimension - width) / 2 + 1;
            var correctorY = (maxRoomYDimension - height) / 2 + 1;
            
            var roomStartPositionX = sector.X * maxRoomXDimension + sector.X + correctorX;
            var roomStartPositionY = sector.Y * maxRoomYDimension + sector.Y + correctorY;

            for (var x = roomStartPositionX; x < width + roomStartPositionX; x++)
            {
                for (var y = roomStartPositionY; y < height + roomStartPositionY; y++)
                {
                    var cell = GetCellByColumnAndRow(cells, x, y);
                    cell.IsVisible = true;
                    result.Cells.Add(cell);
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
            for (var i = 0; i < this.properties.GridDimension.X; i++)
            {
                for (var j = 0; j < this.properties.GridDimension.Y; j++)
                {
                    result[i, j] = true;
                }
            }
        }
        
        private void LinkRooms()
        {
            var width = this.properties.GridDimension.X;
            var height = this.properties.GridDimension.Y;
            
            for (var column = 0; column < width; column++)
            {
                for (var row = 0; row < height; row++)
                {
                    var roomToLink = GetRoomByColumnAndRow(this.rooms, column, row);
                    
                    if (roomToLink == null) continue;
                    
                    roomToLink.NorthernNeighbour = GetNorthernNeighbour(column, row);
                    roomToLink.SouthernNeighbour = GetSouthernNeighbour(column, row);
                    
                    roomToLink.EasternNeighbour = GetEasternNeighbour(column, row);
                    roomToLink.WesternNeighbour = GetWesternNeighbour(column, row);
                }
            }
        }

        private Room<T>? GetWesternNeighbour(int column, int row)
        {
            Room<T>? neighbour = null;
            
            if (column - 1 >= 0)
            {
                neighbour = GetRoomByColumnAndRow(this.rooms, column - 1, row) ??
                            GetWesternNeighbour(column - 1, row);
            }
            
            return neighbour;
        }
        
        private Room<T>? GetEasternNeighbour(int column, int row)
        {
            var width = this.properties.GridDimension.X; 
            Room<T>? neighbour = null;
            
            if (column + 1 <= width)
            {
                neighbour = GetRoomByColumnAndRow(this.rooms, column + 1, row) ??
                            GetEasternNeighbour(column + 1, row);
            }
            
            return neighbour;
        }
        
        private Room<T>? GetNorthernNeighbour(int column, int row)
        {
            
            Room<T>? neighbour = null;
            
            if (row - 1 >= 0)
            {
                neighbour = GetRoomByColumnAndRow(this.rooms, column, row - 1) ??
                            GetNorthernNeighbour(column, row - 1);
            }
            
            return neighbour;
        }

        private Room<T>? GetSouthernNeighbour(int column, int row)
        {
            var height = this.properties.GridDimension.Y;
            Room<T>? neighbour = null;
            
            if (row + 1 <= height)
            {
                neighbour = GetRoomByColumnAndRow(this.rooms, column, row + 1) ??
                            GetSouthernNeighbour(column, row + 1);
            }
            
            return neighbour; 
        }
        

        private void GeneratePaths()
        {
            var unConnectedRooms = this.rooms;

            do
            {
                var nextRoom = GetRandomRoom(unConnectedRooms);
                if (nextRoom != null)
                {
                    nextRoom.Connected = true;
                    if (nextRoom.Neighbours.Values.Any())
                    {
                        var neighbours = new List<Room<T>>(nextRoom.Neighbours.Values!);
                        var randomNeighbour = GetRandomRoom(neighbours);
                    }

                    
                }

                unConnectedRooms = this.rooms.Where(r => !r.Connected).ToList();
            } while (unConnectedRooms.Any());
        }

        private Room<T>? GetRandomRoom(IList<Room<T>> roomList)
        {
            int index = this.random.Next(roomList.Count);
            return roomList[index];
        }
    }
    
     
}