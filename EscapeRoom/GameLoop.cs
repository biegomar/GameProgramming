using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    /// <summary>
    /// Zentraler GameLoop.
    /// </summary>
    internal sealed class GameLoop
    {
        private readonly Coordinate dimension;
        private readonly PlayGround playGround;
       
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="dimension"></param>
        internal GameLoop(Coordinate dimension)  
        { 
            this.dimension = dimension;
            this.playGround = new PlayGround(dimension);
        }

        /// <summary>
        /// Hierüber wird das Spiel gestartet und der eigentliche Game-Loop ausgeführt.
        /// </summary>
        internal void Run()
        {          
            try
            {
                do
                {
                    playGround.NextStep(Console.ReadKey());
                } while (true);
            }
            catch (QuitException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception)
            {

                throw;
            }
        }     
    }
}
