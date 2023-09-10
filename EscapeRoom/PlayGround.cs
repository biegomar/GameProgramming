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
        private Rulez rulez;
        private Coordinate playerPosition;
        private Coordinate keyPosition;
        private Coordinate doorPosition;

        public PlayGround(Coordinate dimension)
        {
            this.rulez = new Rulez(dimension);
            (this.room, this.playerPosition, this.keyPosition, this.doorPosition) = this.rulez;

            this.DrawPlayGround();
        }

        internal void NextStep(ConsoleKeyInfo input)
        {            
            if (input.Key == ConsoleKey.Q)
            {
                throw new QuitException("Ok, das Spiel wird beendet.");
            }

            this.playerPosition = this.rulez.CalculateNewPlayerPositionOnValidRules(input.Key, this.playerPosition);
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
    }
}
