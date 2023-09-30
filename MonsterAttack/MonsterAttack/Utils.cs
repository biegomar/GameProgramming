namespace MonsterAttack
{
    internal static class Utils
    {
        internal const string WantToContinueGame = "Möchtest Du eine weitere Runde spielen (j/n)?";        
        internal const string ErrorMessageEnterValidNumber = "Bitte gibt eine der Zahlen ein, die dir zur Auswahl stehen!";
        internal const string ErrorMessageNumberIsZero = "Bitte eine Zahl größer als 0 eingeben!";
        internal const string HealthPoints = "Wie viele Lebenspunkte soll das Monster haben?";
        internal const string AttackPoints = "Wie viele Angriffsstärke soll das Monster haben?";
        internal const string DefensePoints = "Wie viele Abwehrpunkte soll das Monster haben?";
        internal const string SpeedPoints = "Wie viele Geschwindigkeit soll das Monster haben?";
        internal const string AttackRounds = "Der Kampf dauerte {0} Runden.";
        internal const string StartMessage = "------ Auf gehts! -------";
        internal const string ImpossibleFight = "Die von dir erfassten Werte lassen es nicht zu, dass es einen Sieger gibt!";

        internal static void PrintRulez()
        {
            Console.WriteLine("Lasse zwei Monster verschiedener Monsterklassen gegeneinander antreten!");
            Console.WriteLine("Wähle zunächst die Klasse für das erste Monster aus. Danach erfasst du seine Eigenschaften.");
            Console.WriteLine("(Lebenspunkte - Angriffsstärke - Abwehrpunkte - Geschwindigkeit)");
            Console.WriteLine("Danach erfasst du die selben Daten für das zweite Monster. Aber Achtung! Du kannst keine Monster der selben Klasse erschaffen...");
            Console.WriteLine();
        }

        internal static void PrintLogo()
        {
            Console.WriteLine("");
            Console.WriteLine(" ███▄ ▄███▓ ▒█████   ███▄    █   ██████ ▄▄▄█████▓▓█████  ██▀███      ▄▄▄     ▄▄▄█████▓▄▄▄█████▓ ▄▄▄       ▄████▄   ██ ▄█▀ ▐██▌ ");
            Console.WriteLine("▓██▒▀█▀ ██▒▒██▒  ██▒ ██ ▀█   █ ▒██    ▒ ▓  ██▒ ▓▒▓█   ▀ ▓██ ▒ ██▒   ▒████▄   ▓  ██▒ ▓▒▓  ██▒ ▓▒▒████▄    ▒██▀ ▀█   ██▄█▒  ▐██▌ ");
            Console.WriteLine("▓██    ▓██░▒██░  ██▒▓██  ▀█ ██▒░ ▓██▄   ▒ ▓██░ ▒░▒███   ▓██ ░▄█ ▒   ▒██  ▀█▄ ▒ ▓██░ ▒░▒ ▓██░ ▒░▒██  ▀█▄  ▒▓█    ▄ ▓███▄░  ▐██▌ ");
            Console.WriteLine("▒██    ▒██ ▒██   ██░▓██▒  ▐▌██▒  ▒   ██▒░ ▓██▓ ░ ▒▓█  ▄ ▒██▀▀█▄     ░██▄▄▄▄██░ ▓██▓ ░ ░ ▓██▓ ░ ░██▄▄▄▄██ ▒▓▓▄ ▄██▒▓██ █▄  ▓██▒ ");
            Console.WriteLine("▒██▒   ░██▒░ ████▓▒░▒██░   ▓██░▒██████▒▒  ▒██▒ ░ ░▒████▒░██▓ ▒██▒    ▓█   ▓██▒ ▒██▒ ░   ▒██▒ ░  ▓█   ▓██▒▒ ▓███▀ ░▒██▒ █▄ ▒▄▄  ");
            Console.WriteLine("░ ▒░   ░  ░░ ▒░▒░▒░ ░ ▒░   ▒ ▒ ▒ ▒▓▒ ▒ ░  ▒ ░░   ░░ ▒░ ░░ ▒▓ ░▒▓░    ▒▒   ▓▒█░ ▒ ░░     ▒ ░░    ▒▒   ▓▒█░░ ░▒ ▒  ░▒ ▒▒ ▓▒ ░▀▀▒ ");
            Console.WriteLine("░  ░      ░  ░ ▒ ▒░ ░ ░░   ░ ▒░░ ░▒  ░ ░    ░     ░ ░  ░  ░▒ ░ ▒░     ▒   ▒▒ ░   ░        ░      ▒   ▒▒ ░  ░  ▒   ░ ░▒ ▒░ ░  ░ ");
            Console.WriteLine("░      ░   ░ ░ ░ ▒     ░   ░ ░ ░  ░  ░    ░         ░     ░░   ░      ░   ▒    ░        ░        ░   ▒   ░        ░ ░░ ░     ░ ");
            Console.WriteLine("       ░       ░ ░           ░       ░              ░  ░   ░              ░  ░                       ░  ░░ ░      ░  ░    ░    ");
            Console.WriteLine("                                                                                                         ░                     ");
        }

        internal static void BeepOnWrongEntry()
        {
            Console.Beep(300, 60);
        }
    }    
}
