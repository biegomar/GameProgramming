namespace EscapeRoom
{
    /// <summary>
    /// Central GameLoop.
    /// </summary>
    internal static class GameLoop
    {
        /// <summary>
        /// Real entry point of the game.
        /// </summary>
        internal static void Run()
        {
            do
            {
                Coordinate dimension = GetRoomDimension();
                PlayGround playGround = new PlayGround(dimension);

                Utils.PrintLogo();

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
                Console.Write(Utils.WidthMessage);
                while (!int.TryParse(Console.ReadLine(), out x))
                {
                    Console.WriteLine(Utils.ErrorMessage);
                    Console.Write(Utils.WidthMessage);
                }

                Console.Write(Utils.HeightMessage);
                while (!int.TryParse(Console.ReadLine(), out y))
                {
                    Console.WriteLine(Utils.ErrorMessage);
                    Console.Write(Utils.HeightMessage);
                }

                roomDimension = new Coordinate(x, y);

            } while (!PlayGround.IsRoomDimensionValid(roomDimension));

            return roomDimension;
        }

        private static bool WantToContinueGame(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine("Möchtest Du eine weitere Runde spielen (j/n)?");
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key is ConsoleKey.Y or ConsoleKey.J)
                {
                    Console.WriteLine();
                    return true;
                }
                
                if (key == ConsoleKey.N)
                {
                    return false;
                }

                Utils.BeepOnWrongEntry();
            }                        
        }
    }
}
