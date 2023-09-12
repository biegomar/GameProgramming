using System.Runtime.CompilerServices;

namespace EscapeRoom
{
    internal class Program
    {
        const string ErrorMessage = "Upps, da ist was schief gegangen...bitte nochmal.";
        const string WidthMessage = "Bitte gib die Breite (X-Ausdehnung) des Raumes ein: ";
        const string HeightMessage = "Bitte gib die Höhe (Y-Ausdehnung) des Raumes ein: ";

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
                Console.Write(WidthMessage);

                while (!int.TryParse(Console.ReadLine(), out x))
                {
                    Console.WriteLine(ErrorMessage);
                    Console.Write(WidthMessage);
                }

                Console.Write(HeightMessage);

                while (!int.TryParse(Console.ReadLine(), out y))
                {
                    Console.WriteLine(ErrorMessage);
                    Console.Write(HeightMessage);
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