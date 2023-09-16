using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    internal static class Constants
    {
        internal const string ErrorMessage = "Upps, da ist was schief gegangen...bitte nochmal.";
        internal const string WidthMessage = "Bitte gib die Breite (X-Ausdehnung) des Raumes ein: ";
        internal const string HeightMessage = "Bitte gib die Höhe (Y-Ausdehnung) des Raumes ein: ";

        internal const int OriginalRowPosition = 13; //logo size
        internal const int OriginalColumnPosition = 0;

        internal const char PlayerIcon = 'ß';
        internal const char DoorIcon = '#';
        internal const char KeyIcon = '§';
        internal const char SideWallIcon = '|';
        internal const char TopWallIcon = '-';
        internal const char GroundIcon = '.';

        internal const string YouWin = "█▓▒▒░░░You win!░░░▒▒▓█";

        internal static void PrintLogo()
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
