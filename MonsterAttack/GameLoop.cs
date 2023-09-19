using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterAttack
{
    internal static class GameLoop
    {
        internal static void Run()
        {
            // Outer loop - let me easily "play another round"
            do
            {               
                try
                {
                    // Core game loop!
                    do
                    {
                        
                    } while (true);
                }
                catch (KillException ex)
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
                
            } while (true);
        }

        private static bool WantToContinueGame(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine(Utils.WantToContinueGame);
            do 
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
            } while (true);
        }
    }
}
