using System.Text;

namespace MonsterAttack
{
    /// <summary>
    /// A small utility class.
    /// </summary>
    internal static class Utils
    {
        internal const string WhichRace = "Welche Rasse soll das Monster haben";
        internal const string WantToContinueGame = "Möchtest Du eine weitere Runde spielen (j/n)?";        
        internal const string ErrorMessageEnterValidNumber = "Bitte gibt eine der Zahlen ein, die dir zur Auswahl stehen!";
        internal const string ErrorMessageNumberIsZero = "Bitte eine Zahl größer als 0 eingeben!";
        internal const string HealthPoints = "Wie viele Lebenspunkte soll das Monster haben?";
        internal const string AttackPoints = "Wie viele Angriffsstärke soll das Monster haben?";
        internal const string DefensePoints = "Wie viele Abwehrpunkte soll das Monster haben?";
        internal const string SpeedPoints = "Wie viele Geschwindigkeit soll das Monster haben?";
        internal const string AttackRounds = "Der Kampf dauerte {0} Runden.";
        internal const string ImpossibleFight = "Die von dir erfassten Werte lassen es nicht zu, dass es einen Sieger gibt!";
        internal const string NextRound = "------ Nächste Runde -------";
        internal const string HitNothing = "Das war wohl nix!";
        internal const string VillainIsDead = "Der {0} ist tot!";
        internal const string UuhThatWasHard = "Uhh! Das war heftig! Der {0} hat nur noch {1} Lebenspunkte!";
        internal const string DamageDone = "Der Angriff des {0} verursacht {1} Schaden!";
        internal const string WhoAttackedWhom = "Der {0} attackiert den {1}!";
        
        private const string StartMessage = "------ Auf gehts! -------";
        private static string[] OrkAvatar = new string[40] 
        {
            "",
            "",
            "           ..:;;;;;.                              ",
            "      .;:;;...:.                                  ",
            "  .+;;::.. .+.           ::.:::                   ",
            " :x;::..  .;      :;:. :.    ::;. :+:             ",
            " .:  ..  ..+.      :;:x: .  .:..;;++;:::.         ",
            " .:   .. :::.       :X;  .;.....;+++;;;++;+;.     ",
            "  ;.      ;     .++.;xXxX:...;XXX+;x+;++: .:x.    ",
            "  ..     .:  .:+++;:;;X::x+.++:+Xx++++;++:+++:.   ",
            "   ;  .  .  .xx;:;:.x+xX++xx+xxXx+x;+;Xx;:+:;::.  ",
            "   :. .  :.  +:x++:.:+;$XX++x+$+XX+xXX     .;;+:  ",
            "    ;    :   .. ++x;:+Xxxx;:+X$x+++;X:     ;Xx+;. ",
            "    ..   .   :;:.:x$x++XxxXxxx;:::+x:.      ;:++. ",
            "     . ..;::x+:::;+XXx;:x+x +x+;xxx  ;;;    .:;   ",
            "    .+.  :.;+: ::;;+ ;xXX++x$XXx+X+X;+x:          ",
            "   +x:.:xx;+:: ;:++.;+XXX$X$XXxX$XXXXx:           ",
            "   ;XXx;X; X+X+:x: :++::+XxXx;+$Xx;;:x:           ",
            "    .xx+x+;;: :  ;;:;;;x+Xx+xx+XXX+;++;x;         ",
            "      ::x++   :+;....;;xxx++++xXXX$Xx+;.::.       ",
            "         +;  ;x.::+;;;+$Xxx;++++ +$Xxxx+;;X.      ",
            "            :+.:...;$$: x+x;+;+Xx+;+xx+..::       ",
            "           ;+X+.:+X$Xx  ;;;;;;;xX:.:;xx+:         ",
            "          .;;x;x$X;:;x; ;;++++++Xx+x.             ",
            "            :+. +:..::+ :+;++::Xx:;;              ",
            "             :: ....:;  ..    ;+x;;+;             ",
            "              +.::.+;         +:;x+;;;;;          ",
            "              .;:+;x.         ;+:::xxx+;+.        ",
            "              x$x++x+           .:;;;;++:         ",
            "             :;.:.:+x                             ",
            "           ;+x++;;;+x:                            ",
            "          ;;:;;x;;;;x+.                           ",
            "         ::+x++;;;++x::                           ",
            "         ;x;+:: +:  :x:                           ",
            "          .;;;+:++:.+:                            ",
            "             ..:  :;                              ",
            "                                                  ",
            "                                                  ",
            "",
            ""
        };

        private static string[] GoblinAvatar = new string[40]
        {
            "",
            "                 .::-:.         .....             ",
            "      .......:-----=+**+-.::.:======:             ",
            "       .:-=+*=:=-..=++**==+++**=:.                ",
            "         .=#*==--*==*+**=*=+=+.                   ",
            "           -*- ==+*+=+**+=++=.                    ",
            "           .--=***=+***###-.                      ",
            "            .-**+*++***##*=                       ",
            "             .+++***#####**+-.                    ",
            "              ++*#####*##*++*****+-               ",
            "             .-.-==++***+==-==..:=**:             ",
            "             :.:-::-++=-:.. :==--=+#+.            ",
            "            .=++=:  :-   .:-=*#%#**##-.           ",
            "            -+-##=..-......=#%%%+.:=#*:.          ",
            "           :+.:#%#**: :++*##%%%%*: :+#+.          ",
            "          :*+-+%==+==------+#%%#----=***-         ",
            "         --+***#.=*=...-+++*#%%*. =**=-+*-        ",
            "        .:.=+-+#:+:   .-=*####%*. -:+- =*+        ",
            "        :--*==*===. .::-=+**####- .==-:=#*.       ",
            "        :++#++*.+=.-::.:==*#####+. :=--+#*.       ",
            "        :+*##*..++-*+::-=+*##*###- .-++*##:       ",
            "        :+#%#:  -#+*#+=++*#%%%%###. :+*###-       ",
            "        :+*##: .=##%#####%%#%%*+*#+.-=+#%%+.      ",
            "        ::+#%=.--+####**###%%#==*##:-+###%#:      ",
            "       .:+%%#*-:=+#####**#%%#*=-+##==#%%%%#-      ",
            "       .-#%%##-:+*##%#####%#%#+-+*##+#*#%%#:      ",
            "       .=+###*-=*##%%#####*-*#*=+##%***#%#*:      ",
            "       .-#+##==###%#:*####*..=%#+**##+*##*:       ",
            "         :#=+=+%%##%#+#%##*.  .#+--+*###*.        ",
            "          ..:-*##*##%+.        =##*##%+.          ",
            "              .*####%*.        :*#**#%%.          ",
            "               .*###%#:         :*#**%%:          ",
            "                :*##%%=          .*#*#%:          ",
            "               =-=+#%%#:          :#*##+          ",
            "           ..=--+=*####*.         :-+**#=         ",
            "        .-====*++####*+-        ..+-=***#+.       ",
            "       :==*+=*####+-.           .=--+++=+*=:      ",
            "        ..:---.                 .:-=+++*=--:.     ",
            "                                    ......        ",
            "                                       ..."
        };

        private static string[] TrollAvatar = new string[40]
        {
            "",
            "",
            "",
            "                           ...:;:                           ",
            "                       .;+XXXXXXXXx:.                       ",
            "                 .:++xXXXxxxXXx+xXxx+xxx...                 ",
            "               :XXxXxXXXxxXXxxxXx+xxx++xx;                  ",
            "              .$XXXXxxXXXXxxxxxxxx+xXXxx++.                 ",
            "             .xXXxxxxXXXXxxxxxxxx+XXXX$xx+;:                ",
            "            xxxXXXxxxxxx++xxxxxXXxxXXXXX+xXX;               ",
            "           +XxxxxxX$$Xx+++++xxXX$xx$xxxXx$$XX;              ",
            "         :xXXxxxXX$&$$Xxx+++xxxXXxXXXXxXXXXXX;              ",
            "       .xxxxxXX$$$$&$$XxxxxxxxxXX$XX$$XXXXXx:               ",
            "       +Xx+xX$$+ $$$$XXXXXXXXX$$XX$&$$X;                    ",
            "      .XxxxxXXX  X$$$XXXX$$$$$X$$$$$$X+                     ",
            "      ;XxxxxXX;  X$$$$$$$$$$$$$$$$x$$X;                     ",
            "      .XxxxxX:  :$$$$$$$$$$$$$$$$Xx$$X+                     ",
            "       XXxxxx   +$$$X$$XXX$$X$$$$X$XXX:                     ",
            "      .Xx++xX;  :$$XXXxXXXX$X$$$$XX$$x                      ",
            "      xx++xxX$; +XXXxXxxXXXXXXXXXXX&&x                      ",
            "     .XXX$X+xX  x$$XXxxxxxxXXXXX$XX$$X+                     ",
            "     ;$$$$$$X: .$XXXXX$XXXXX$$$$$$XxxXX:                    ",
            "   :X$$X       :$X++XXX$$$X$$$$$$XXxXX&$+.                  ",
            " .+X$X:        XXXxxxX$$$$$X$$$$XXxXX$;.+$x+.      .+;      ",
            "+xXx.          $XXXXX$$+x$$$XX$$$$XXX$X   :x$x+:  +xXXXx+.  ",
            "+;.            +XXXX$X: ;$$$XX.;$$$X$XX.     ;$$XXXXXXXXXXx:",
            "               XXXX$$X. :$$$X:;$$$$$$$$.       x$XXXXXXXX$$;",
            "              ;$$$$$$$X  +;::.$$$$$$$X;       .$$XXXXXX$$$+ ",
            "               +$$$$$$;     .X$$$$X;.         $$$$$$$X$$$+  ",
            "                 +$$$:    +X$&&x.             x$$$$$X$$&;   ",
            "                  ;$$$.   :;$$$$X;           .  :$$$$$$:    ",
            "                .+XXXXX:::;+x$$XXX$X$XXxx++++++xxxxxx+;:.   ",
            "             ;x$XX$XXXX$$XXXXxxxxxxx++;:.                   ",
            "             ::;:;;;;:;;:......   ",
            "",
            "",
            "",
            "",
            "",
            ""
        };

        /// <summary>
        /// Print the start screen.
        /// </summary>
        /// <param name="firstOpponent">The first monster.</param>
        /// <param name="secondOpponent">The second monster.</param>
        internal static void PrintStart(Monster firstOpponent, Monster secondOpponent)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(StartMessage);
            Console.WriteLine();            
            Console.Write(CombineAvatars(firstOpponent, secondOpponent));            
        }

        /// <summary>
        /// Print the rules.
        /// </summary>
        internal static void PrintRulez()
        {
            Console.WriteLine("Lasse zwei Monster verschiedener Monsterklassen gegeneinander antreten!");
            Console.WriteLine("Wähle zunächst die Klasse für das erste Monster aus. Danach erfasst du seine Eigenschaften.");
            Console.WriteLine("(Lebenspunkte - Angriffsstärke - Abwehrpunkte - Geschwindigkeit)");
            Console.WriteLine("Danach erfasst du die selben Daten für das zweite Monster. Aber Achtung! Du kannst keine Monster der selben Klasse erschaffen...");
            Console.WriteLine();
        }

        /// <summary>
        /// Print the logo.
        /// </summary>
        internal static void PrintLogo()
        {
            Console.Clear();
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

        /// <summary>
        /// Plays a funny beep.
        /// </summary>
        internal static void BeepOnWrongEntry()
        {
            Console.Beep(300, 60);
        }

        /// <summary>
        /// Returns the specific monster avatar, based on the monster class.
        /// </summary>
        /// <param name="monsterClass">The monster class.</param>
        /// <returns>The specific avatar.</returns>
        internal static string[] GetAvatarForMonsterClass(MonsterClass monsterClass)
        {
            return monsterClass switch
            {
                MonsterClass.Ork => OrkAvatar,
                MonsterClass.Troll => TrollAvatar,
                MonsterClass.Goblin => GoblinAvatar,
                _ => Array.Empty<string>(),
            };
        }

        /// <summary>
        /// Generates a combined output for both monsters.
        /// </summary>
        /// <param name="firstOpponent">The first monster.</param>
        /// <param name="secondOpponent">The second monster.</param>
        /// <returns></returns>
        private static string CombineAvatars(Monster firstOpponent, Monster secondOpponent)
        {
            var length = Math.Max(firstOpponent.Avatar.Length, secondOpponent.Avatar.Length);
            var result = new StringBuilder(length);
            var paddingForFirstOpponent = firstOpponent.Avatar.OrderByDescending(s => s.Length).First().Length + 2;

            result.AppendLine(GetCenteredHeadline(firstOpponent, secondOpponent));

            for (int i = 0; i < length; i++)
            {
                result.AppendLine(firstOpponent.Avatar[i].PadRight(paddingForFirstOpponent, ' ') + " " + secondOpponent.Avatar[i]);
            }

            result.AppendLine();

            var dataLineFirstOpponent = $"Attack: {firstOpponent.AP} - Defense: {firstOpponent.DP} - Speed: {firstOpponent.S} - Health: {firstOpponent.HP}";
            var dataLineSecondOpponent = $"Attack: {secondOpponent.AP} - Defense: {secondOpponent.DP} - Speed: {secondOpponent.S} - Health: {secondOpponent.HP}";
            result.AppendLine(dataLineFirstOpponent.PadRight(paddingForFirstOpponent, ' ') + " " + dataLineSecondOpponent);

            return result.ToString();
        }

        /// <summary>
        /// Prints a centered headline.
        /// </summary>
        /// <param name="firstOpponent">The first monster.</param>
        /// <param name="secondOpponent">The second monster.</param>
        /// <returns></returns>
        private static string GetCenteredHeadline(Monster firstOpponent, Monster secondOpponent)
        {
            var paddingForFirstOpponent = firstOpponent.Avatar.OrderByDescending(s => s.Length).First().Length + 2;
            var paddingForSecondOpponent = secondOpponent.Avatar.OrderByDescending(s => s.Length).First().Length;

            return firstOpponent.R.ToString().PadLeft(paddingForFirstOpponent / 2, ' ').PadRight(paddingForFirstOpponent, ' ') + secondOpponent.R.ToString().PadLeft(paddingForSecondOpponent / 2, ' ');
        }
    }    
}
