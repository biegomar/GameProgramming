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


        internal static void PrintRulez()
        {
            Console.WriteLine();
            Console.WriteLine("Du stehst allein auf weiter Flur und versucht aus dem Raum, in dem Du Dich befindest zu entkommen.");
            Console.WriteLine("Doch so einfach ist es nicht. Die Tür, die Du entdeckst ist verschlossen und lässt sich nur über einen Schlüssel öffnen. Such den Schlüssel!");
            Console.WriteLine();
            Console.WriteLine("Spielaufbau");
            Console.WriteLine("-----------");
            Console.WriteLine();
            Console.WriteLine("Folgende Items kannst Du auf dem Spielfeld erkennen:");
            Console.WriteLine($"{Constants.PlayerIcon} - der Spieler (also Du!)");
            Console.WriteLine($"{Constants.DoorIcon} - die Tür. Sie befindet sich irgendwo auf dem Rand des Spielfelds. Ist sie rot, ist sie noch verschlossen. Ansonsten ist sie grün und kann passiert werden.");
            Console.WriteLine($"{Constants.KeyIcon} - der Schlüssel. Finde ihn und der Weg ist frei.");
            Console.WriteLine();
            Console.WriteLine("Steuerung");
            Console.WriteLine("---------");
            Console.WriteLine();
            Console.WriteLine("Du kannst den Spieler (also Dich) mit Hilfe der folgenden Tasten bewegen:");
            Console.WriteLine($"{ConsoleKey.UpArrow} oder {ConsoleKey.W} - bewegen dich nach oben");
            Console.WriteLine($"{ConsoleKey.DownArrow} oder {ConsoleKey.S} - bewegen dich nach unten");
            Console.WriteLine($"{ConsoleKey.RightArrow} oder {ConsoleKey.D} - bewegen dich nach rechts");
            Console.WriteLine($"{ConsoleKey.LeftArrow} oder {ConsoleKey.A} - bewegen dich nach links");
            Console.WriteLine();
        }

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
