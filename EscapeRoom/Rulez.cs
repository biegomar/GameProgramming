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
        private Coordinate? dimension;
        private Coordinate? keyPosition;
        private Coordinate? doorPosition;

        internal static bool IsRoomDimensionValid(Coordinate roomDimension)
        {
            if (roomDimension.X <= 0 || roomDimension.Y <= 0)
            {
                Console.WriteLine("Der Raum benötigt eine Länge und eine Breite > 0.");
                return false;
            }
            else if (roomDimension.X > 25 || roomDimension.Y > 25)
            {
                Console.WriteLine("Der Raum sollte die Länge oder die Breite von 25 nicht überschreiten.");
                return false;
            }

            return true;
        }

        internal (char[,] room, Coordinate playerPosition, Coordinate keyPosition, Coordinate doorPosition) InitialzeEnvironmentOnValidRules(Coordinate dimension)
        {
            this.dimension = dimension;
            return (this.GenerateRoom(dimension), this.GetPlayerPosition(), this.GetKeyPosition(), this.GetDoorPosition());
        }

        internal Coordinate CalculateNewPlayerPositionOnValidRules(ConsoleKey input, Coordinate playerPosition)
        {
            return playerPosition;
        }

        private char[,] GenerateRoom(Coordinate dimension)
        {
            return new char[dimension.X - 1, dimension.Y - 1];
        }

        private Coordinate GetPlayerPosition()
        {
            return new Coordinate(1, 1);
        }

        private Coordinate GetKeyPosition()
        {
            this.keyPosition = new Coordinate(2, 2);
            return this.keyPosition;
        }

        private Coordinate GetDoorPosition()
        {
            this.doorPosition = new Coordinate(0, 0);
            return this.doorPosition;
        }
    }
}
