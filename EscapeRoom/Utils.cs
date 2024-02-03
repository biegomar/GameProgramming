namespace EscapeRoom
{
    /// <summary>
    /// A small utility class.
    /// </summary>
    internal static class Utils
    {
        internal const string ErrorMessage = "Upps, da ist was schief gegangen...bitte nochmal.";
        internal const string WidthMessage = "Bitte gib die Breite (X-Ausdehnung) des Raumes ein: ";
        internal const string HeightMessage = "Bitte gib die Höhe (Y-Ausdehnung) des Raumes ein: ";
        internal const string WantToContinueGame = "Möchtest Du eine weitere Runde spielen (j/n)?";
        internal const string QuitGame = "Ok, das Spiel wird beendet.";
        internal const string EnterRightPlayGroundDimension = "Der Raum benötigt eine Breite und eine Höhe > 0.";

        internal const string RoomDimensionNotValid =
            "Der Raum sollte die maximale Breite ({0}) oder die maximale Höhe ({1}) nicht überschreiten.";

        internal const int OriginalRowPosition = 13; //logo size
        internal const int OriginalColumnPosition = 0;

        internal const char PlayerIcon = 'ß';
        internal const char DoorIcon = '#';
        internal const char KeyIcon = '§';
        internal const char SideWallIcon = '|';
        internal const char TopWallIcon = '-';
        internal const char GroundIcon = '.';

        internal const string YouWin = "█▓▒▒░░░You win!░░░▒▒▓█";
        
        /// <summary>
        /// Prints the rulez.
        /// </summary>
        internal static void PrintRulez()
        {
            Console.WriteLine();
            Console.WriteLine("Du stehst allein auf weiter Flur und versucht aus dem Raum in dem Du Dich befindest zu entkommen.");
            Console.WriteLine("Doch so einfach ist es nicht. Die Tür, die Du entdeckst ist verschlossen und lässt sich nur über einen Schlüssel öffnen. Such den Schlüssel!");
            Console.WriteLine();
            Console.WriteLine("Spielaufbau");
            Console.WriteLine("-----------");
            Console.WriteLine();
            Console.WriteLine("Folgende Items kannst Du auf dem Spielfeld erkennen:");
            Console.WriteLine($"{PlayerIcon} - der Spieler (also Du!)");
            Console.WriteLine($"{DoorIcon} - die Tür. Sie befindet sich irgendwo auf dem Rand des Spielfelds. Ist sie rot, ist sie noch verschlossen. Ansonsten ist sie grün und kann passiert werden.");
            Console.WriteLine($"{KeyIcon} - der Schlüssel. Finde ihn und der Weg ist frei.");
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

        /// <summary>
        /// Prints to logo.
        /// </summary>
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

        /// <summary>
        /// Plays a funny beep.
        /// </summary>
        internal static void BeepOnWrongEntry()
        {
            Console.Beep(300, 60);
        }
    }
}
