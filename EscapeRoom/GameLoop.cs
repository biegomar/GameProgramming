﻿using System;
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
        private readonly PlayGround playGround;
       
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="dimension"></param>
        internal GameLoop(Coordinate dimension)  
        { 
            this.playGround = new PlayGround(dimension);
        }

        /// <summary>
        /// Hierüber wird das Spiel gestartet und der eigentliche Game-Loop ausgeführt.
        /// </summary>
        internal void Run()
        {
            playGround.DrawInitialPlayGround();

            try
            {
                do
                {
                    playGround.NextStep(Console.ReadKey(true));
                } while (true);
            }
            catch (QuitException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (WinException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            playGround.CleanUp();
        }     
    }
}
