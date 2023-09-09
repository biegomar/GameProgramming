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
        private Vector? dimension;
        private Vector? keyPosition;
        private Vector? doorPosition;

        internal static bool IsRoomDimensionValid(Vector roomDimension)
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

        internal (char[,] room, Vector playerPosition, Vector keyPosition, Vector doorPosition) InitialzeEnvironmentOnValidRules(Vector dimension)
        {
            this.dimension = dimension;
            return (this.GenerateRoom(dimension), this.GetPlayerPosition(), this.GetKeyPosition(), this.GetDoorPosition());
        }

        internal Vector CalculateNewPlayerPositionOnValidRules(ConsoleKey input, Vector playerPosition)
        {
            return playerPosition;
        }

        private char[,] GenerateRoom(Vector dimension)
        {
            return new char[dimension.X - 1, dimension.Y - 1];
        }

        private Vector GetPlayerPosition()
        {
            return new Vector(1, 1);
        }

        private Vector GetKeyPosition()
        {
            this.keyPosition = new Vector(2, 2);
            return this.keyPosition;
        }

        private Vector GetDoorPosition()
        {
            this.doorPosition = new Vector(0, 0);
            return this.doorPosition;
        }
    }
}
