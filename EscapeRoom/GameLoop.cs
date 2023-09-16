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
    internal static class GameLoop
    {
 
        /// <summary>
        /// Hierüber wird das Spiel gestartet und der eigentliche Game-Loop ausgeführt.
        /// </summary>
        internal static void Run()
        {            
            Coordinate dimension = GetRoomDimension();
            PlayGround playGround = new PlayGround(dimension);

            Constants.PrintLogo();

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

        private static Coordinate GetRoomDimension()
        {
            int x = 0;
            int y = 0;

            Coordinate roomDimension = new(x, y);

            do
            {
                Console.Write(Constants.WidthMessage);

                while (!int.TryParse(Console.ReadLine(), out x))
                {
                    Console.WriteLine(Constants.ErrorMessage);
                    Console.Write(Constants.WidthMessage);
                }

                Console.Write(Constants.HeightMessage);

                while (!int.TryParse(Console.ReadLine(), out y))
                {
                    Console.WriteLine(Constants.ErrorMessage);
                    Console.Write(Constants.HeightMessage);
                }

                roomDimension = new(x, y);

            } while (!PlayGround.IsRoomDimensionValid(roomDimension));

            return roomDimension;
        }
    }
}
