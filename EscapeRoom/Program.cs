using System.Runtime.CompilerServices;

namespace EscapeRoom
{
    internal class Program
    {      
        static void Main(string[] args)
        {
            PrintIntro();

            var gameLoop = new GameLoop(GetRoomDimension());

            PrintLogo();

            gameLoop.Run();
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

        private static void PrintIntro() 
        { 
            PrintLogo();
        }

        private static void PrintLogo()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine("▓█████   ██████  ▄████▄   ▄▄▄       ██▓███  ▓█████  ██▀███   ▒█████   ▒█████   ███▄ ▄███▓");
            Console.WriteLine("▓█   ▀ ▒██    ▒ ▒██▀ ▀█  ▒████▄    ▓██░  ██▒▓█   ▀ ▓██ ▒ ██▒▒██▒  ██▒▒██▒  ██▒▓██▒▀█▀ ██▒");
            Console.WriteLine("▒███   ░ ▓██▄   ▒▓█    ▄ ▒██  ▀█▄  ▓██░ ██▓▒▒███   ▓██ ░▄█ ▒▒██░  ██▒▒██░  ██▒▓██    ▓██░");
            Console.WriteLine("▒▓█  ▄   ▒   ██▒▒▓▓▄ ▄██▒░██▄▄▄▄██ ▒██▄█▓▒ ▒▒▓█  ▄ ▒██▀▀█▄  ▒██   ██░▒██   ██░▒██    ▒██ ");
            Console.WriteLine("░▒████▒▒██████▒▒▒ ▓███▀ ░ ▓█   ▓██▒▒██▒ ░  ░░▒████▒░██▓ ▒██▒░ ████▓▒░░ ████▓▒░▒██▒   ░██▒");
            Console.WriteLine("░░ ▒░ ░▒ ▒▓▒ ▒ ░░ ░▒ ▒  ░ ▒▒   ▓▒█░▒▓▒░ ░  ░░░ ▒░ ░░ ▒▓ ░▒▓░░ ▒░▒░▒░ ░ ▒░▒░▒░ ░ ▒░   ░  ░");
            Console.WriteLine(" ░ ░  ░░ ░▒  ░ ░  ░  ▒     ▒   ▒▒ ░░▒ ░      ░ ░  ░  ░▒ ░ ▒░  ░ ▒ ▒░   ░ ▒ ▒░ ░  ░      ░");
            Console.WriteLine("   ░   ░  ░  ░  ░          ░   ▒   ░░          ░     ░░   ░ ░ ░ ░ ▒  ░ ░ ░ ▒  ░      ░   ");
            Console.WriteLine("   ░  ░      ░  ░ ░            ░  ░            ░  ░   ░         ░ ░      ░ ░         ░   ");
            Console.WriteLine("                ░                                                                        ");
            Console.WriteLine();
        }
    }
}