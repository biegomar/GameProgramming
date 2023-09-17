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
            do
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
                    Environment.Exit(0);
                }
                catch (WinException ex)
                {
                    if (!WantToContinueGame(ex.Message))
                    {
                        Environment.Exit(0);
                    }                                       
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Environment.Exit(0);
                }

                playGround.CleanUp();
            } while (true);
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

        private static bool WantToContinueGame(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine("Möchtest Du eine weitere Runde spielen?");
            if (Console.ReadKey(true).Key is ConsoleKey.Y or ConsoleKey.J)
            {
                Console.WriteLine();
                return true;
            }

            return false;
        }
    }
}
