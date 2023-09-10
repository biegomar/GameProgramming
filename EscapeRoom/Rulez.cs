using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    internal sealed class Rulez
    {
        private readonly Coordinate dimension;
        private Coordinate KeyPosition;
        private Coordinate DoorPosition;
        private Coordinate InitialPlayerPosition;
        private char[,] Room;

        internal Rulez(Coordinate roomDimension)
        {
            this.dimension = roomDimension;
            this.InitialPlayerPosition = this.GetPlayerPosition();
            this.KeyPosition = this.GetKeyPosition();
            this.DoorPosition = this.GetDoorPosition();
            this.Room = this.GenerateRoom();
        }

        internal void Deconstruct(out char[,] room, out Coordinate initialPlayerPosition, out Coordinate keyPosition, out Coordinate doorPosition)
        {
            room = this.Room;
            initialPlayerPosition = this.InitialPlayerPosition;
            keyPosition = this.KeyPosition;
            doorPosition = this.DoorPosition;
        }

        internal static bool IsRoomDimensionValid(Coordinate roomDimension)
        {
            int maxDimensionX = 50;
            int maxDimensionY = 25;

            if (roomDimension.X <= 0 || roomDimension.Y <= 0)
            {
                Console.WriteLine("Der Raum benötigt eine Breite und eine Höhe > 0.");
                return false;
            }
            else if (roomDimension.X > maxDimensionX || roomDimension.Y > maxDimensionY)
            {
                Console.WriteLine($"Der Raum sollte die maximale Breite ({maxDimensionX}) oder die maximale Höhe ({maxDimensionY}) nicht überschreiten.");
                return false;
            }

            return true;
        }        

        internal Coordinate CalculateNewPlayerPositionOnValidRules(ConsoleKey input, Coordinate playerPosition)
        {
            return playerPosition;
        }

        private char[,] GenerateRoom()
        {
            var roomDefinition = new char[this.dimension.X, this.dimension.Y];

            for (int row = 0; row < this.dimension.Y; row++)
            {
                for (int column = 0; column < this.dimension.X; column++)
                {
                    roomDefinition[column, row] = GetRoomPositionContent(column, row);
                }
            }

            return roomDefinition;
        }

        private char GetRoomPositionContent(int column, int row)
        {
            if (column == this.InitialPlayerPosition.X && row == this.InitialPlayerPosition.Y)
            {
                return 'ß';
            }
            else if (column == this.DoorPosition.X && row == this.DoorPosition.Y)            
            {
                return '|';
            }
            else if (column == this.KeyPosition.X && row == this.KeyPosition.Y)
            {
                return '§';
            }

            return '.';
        }

        private Coordinate GetPlayerPosition()
        {
            return new Coordinate(1, 1);
        }

        private Coordinate GetKeyPosition()
        {
            return new Coordinate(2, 2);
        }

        private Coordinate GetDoorPosition()
        {
            return new Coordinate(0, 0);
        }
    }
}
