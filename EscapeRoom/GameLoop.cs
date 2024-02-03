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
            // Outer loop - let me easily "play another round"
            do
            {
                Vector2D dimension = GetRoomDimension();
                PlayGround playGround = new (dimension);

                Utils.PrintLogo();

                playGround.DrawInitialPlayGround();

                try
                {
                    // Core game loop!
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
                    //catch unexpected exceptions.
                    Console.WriteLine(ex.Message);
                    Environment.Exit(0);
                }

                playGround.CleanUp();
            } while (true);
        }

        /// <summary>
        /// Ask the player for the room dimensions.
        /// </summary>
        /// <returns>The dimension of the room as a vector.</returns>
        private static Vector2D GetRoomDimension()
        {
            int x = 0;
            int y = 0;

            Vector2D roomDimension;

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

                roomDimension = new Vector2D(x, y);

            } while (!PlayGround.IsRoomDimensionValid(roomDimension));

            return roomDimension;
        }

        /// <summary>
        /// Maybe the player wants to play another round
        /// </summary>
        /// <param name="message"></param>
        /// <returns>True if the player wants to play again, false otherwise.</returns>
        private static bool WantToContinueGame(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine(Utils.WantToContinueGame);
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
