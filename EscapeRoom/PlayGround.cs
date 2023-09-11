using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    internal sealed class PlayGround
    {
        private char[,] room;
        private Coordinate playerPosition;
        private Coordinate keyPosition;
        private Coordinate doorPosition;

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

        public PlayGround(Coordinate dimension)
        {
            this.playerPosition = this.GetPlayerPosition();
            this.doorPosition = this.GetDoorPosition();
            this.keyPosition = this.GetKeyPosition();

            this.room = this.GenerateRoom(dimension);

            this.DrawPlayGround();
        }

        internal void NextStep(ConsoleKeyInfo input)
        {            
            if (input.Key == ConsoleKey.Q)
            {
                throw new QuitException("Ok, das Spiel wird beendet.");
            }

            this.playerPosition = this.CalculateNewPlayerPositionOnValidRules(input.Key, this.playerPosition);
        }

        private Coordinate CalculateNewPlayerPositionOnValidRules(ConsoleKey input, Coordinate playerPosition)
        {
            return playerPosition;
        }

        private void DrawPlayGround()
        {
            int numberOfRows = room.GetLength(1);
            int numberOfColumns = room.GetLength(0);

            for (int row = 0; row < numberOfRows; row++)
            {
                for (int column = 0; column < numberOfColumns; column++)
                {
                    Console.Write(room[column, row]);
                }
                Console.WriteLine();
            }           
        }

        private Coordinate GetPlayerPosition()
        {
            return new Coordinate(1, 1);
        }

        private Coordinate GetDoorPosition()
        {
            return new Coordinate(0, 0);
        }

        private Coordinate GetKeyPosition()
        {
            return new Coordinate(2, 2);
        }

        private char[,] GenerateRoom(Coordinate dimension)
        {
            var roomDefinition = new char[dimension.X, dimension.Y];

            for (int row = 0; row < dimension.Y; row++)
            {
                for (int column = 0; column < dimension.X; column++)
                {
                    roomDefinition[column, row] = GetRoomPositionContent(column, row);
                }
            }

            return roomDefinition;
        }

        private char GetRoomPositionContent(int column, int row)
        {
            if (column == this.playerPosition.X && row == this.playerPosition.Y)
            {
                return 'ß';
            }
            else if (column == this.doorPosition.X && row == this.doorPosition.Y)
            {
                return '|';
            }
            else if (column == this.keyPosition.X && row == this.keyPosition.Y)
            {
                return '§';
            }

            return '.';
        }
    }
}
