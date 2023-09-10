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
            this.rulez = new Rulez();
            (this.room, this.playerPosition, this.keyPosition, this.doorPosition) = this.rulez.InitialzeEnvironmentOnValidRules(dimension);
        }

        internal void NextStep(ConsoleKeyInfo input)
        {            
            if (input.Key == ConsoleKey.Q)
            {
                throw new QuitException("Ok, das Spiel wird beendet.");
            }

            this.playerPosition = this.rulez.CalculateNewPlayerPositionOnValidRules(input.Key, this.playerPosition);
        }            
    }
}
