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

        internal const string OrkAvatar = @"                                                  
                                                  
           ..:;;;;;.                              
      .;:;;...:.                                  
  .+;;::.. .+.           ::.:::                   
 :x;::..  .;      :;:. :.    ::;. :+:             
 .:  ..  ..+.      :;:x: .  .:..;;++;:::.         
 .:   .. :::.       :X;  .;.....;+++;;;++;+;.     
  ;.      ;     .++.;xXxX:...;XXX+;x+;++: .:x.    
  ..     .:  .:+++;:;;X::x+.++:+Xx++++;++:+++:.   
   ;  .  .  .xx;:;:.x+xX++xx+xxXx+x;+;Xx;:+:;::.  
   :. .  :.  +:x++:.:+;$XX++x+$+XX+xXX     .;;+:  
    ;    :   .. ++x;:+Xxxx;:+X$x+++;X:     ;Xx+;. 
    ..   .   :;:.:x$x++XxxXxxx;:::+x:.      ;:++. 
     . ..;::x+:::;+XXx;:x+x +x+;xxx  ;;;    .:;   
    .+.  :.;+: ::;;+ ;xXX++x$XXx+X+X;+x:          
   +x:.:xx;+:: ;:++.;+XXX$X$XXxX$XXXXx:           
   ;XXx;X; X+X+:x: :++::+XxXx;+$Xx;;:x:           
    .xx+x+;;: :  ;;:;;;x+Xx+xx+XXX+;++;x;         
      ::x++   :+;....;;xxx++++xXXX$Xx+;.::.       
         +;  ;x.::+;;;+$Xxx;++++ +$Xxxx+;;X.      
            :+.:...;$$: x+x;+;+Xx+;+xx+..::       
           ;+X+.:+X$Xx  ;;;;;;;xX:.:;xx+:         
          .;;x;x$X;:;x; ;;++++++Xx+x.             
            :+. +:..::+ :+;++::Xx:;;              
             :: ....:;  ..    ;+x;;+;             
              +.::.+;         +:;x+;;;;;          
              .;:+;x.         ;+:::xxx+;+.        
              x$x++x+           .:;;;;++:         
             :;.:.:+x                             
           ;+x++;;;+x:                            
          ;;:;;x;;;;x+.                           
         ::+x++;;;++x::                           
         ;x;+:: +:  :x:                           
          .;;;+:++:.+:                            
             ..:  :;                              
                                                  
                                                  ";

        internal const string GoblinAvatar = @"                                                  
                 .::-:.         .....             
      .......:-----=+**+-.::.:======:             
       .:-=+*=:=-..=++**==+++**=:.                
         .=#*==--*==*+**=*=+=+.                   
           -*- ==+*+=+**+=++=.                    
           .--=***=+***###-.                      
            .-**+*++***##*=                       
             .+++***#####**+-.                    
              ++*#####*##*++*****+-               
             .-.-==++***+==-==..:=**:             
             :.:-::-++=-:.. :==--=+#+.            
            .=++=:  :-   .:-=*#%#**##-.           
            -+-##=..-......=#%%%+.:=#*:.          
           :+.:#%#**: :++*##%%%%*: :+#+.          
          :*+-+%==+==------+#%%#----=***-         
         --+***#.=*=...-+++*#%%*. =**=-+*-        
        .:.=+-+#:+:   .-=*####%*. -:+- =*+        
        :--*==*===. .::-=+**####- .==-:=#*.       
        :++#++*.+=.-::.:==*#####+. :=--+#*.       
        :+*##*..++-*+::-=+*##*###- .-++*##:       
        :+#%#:  -#+*#+=++*#%%%%###. :+*###-       
        :+*##: .=##%#####%%#%%*+*#+.-=+#%%+.      
        ::+#%=.--+####**###%%#==*##:-+###%#:      
       .:+%%#*-:=+#####**#%%#*=-+##==#%%%%#-      
       .-#%%##-:+*##%#####%#%#+-+*##+#*#%%#:      
       .=+###*-=*##%%#####*-*#*=+##%***#%#*:      
       .-#+##==###%#:*####*..=%#+**##+*##*:       
         :#=+=+%%##%#+#%##*.  .#+--+*###*.        
          ..:-*##*##%+.        =##*##%+.          
              .*####%*.        :*#**#%%.          
               .*###%#:         :*#**%%:          
                :*##%%=          .*#*#%:          
               =-=+#%%#:          :#*##+          
           ..=--+=*####*.         :-+**#=         
        .-====*++####*+-        ..+-=***#+.       
       :==*+=*####+-.           .=--+++=+*=:      
        ..:---.                 .:-=+++*=--:.     
                                    ......        
                                       ...";

        internal const string TrollAvatar = @"
                           ...:;:                           
                       .;+XXXXXXXXx:.                       
                 .:++xXXXxxxXXx+xXxx+xxx...                 
               :XXxXxXXXxxXXxxxXx+xxx++xx;                  
              .$XXXXxxXXXXxxxxxxxx+xXXxx++.                 
             .xXXxxxxXXXXxxxxxxxx+XXXX$xx+;:                
            xxxXXXxxxxxx++xxxxxXXxxXXXXX+xXX;               
           +XxxxxxX$$Xx+++++xxXX$xx$xxxXx$$XX;              
         :xXXxxxXX$&$$Xxx+++xxxXXxXXXXxXXXXXX;              
       .xxxxxXX$$$$&$$XxxxxxxxxXX$XX$$XXXXXx:               
       +Xx+xX$$+ $$$$XXXXXXXXX$$XX$&$$X;                    
      .XxxxxXXX  X$$$XXXX$$$$$X$$$$$$X+                     
      ;XxxxxXX;  X$$$$$$$$$$$$$$$$x$$X;                     
      .XxxxxX:  :$$$$$$$$$$$$$$$$Xx$$X+                     
       XXxxxx   +$$$X$$XXX$$X$$$$X$XXX:                     
      .Xx++xX;  :$$XXXxXXXX$X$$$$XX$$x                      
      xx++xxX$; +XXXxXxxXXXXXXXXXXX&&x                      
     .XXX$X+xX  x$$XXxxxxxxXXXXX$XX$$X+                     
     ;$$$$$$X: .$XXXXX$XXXXX$$$$$$XxxXX:                    
   :X$$X       :$X++XXX$$$X$$$$$$XXxXX&$+.                  
 .+X$X:        XXXxxxX$$$$$X$$$$XXxXX$;.+$x+.      .+;      
+xXx.          $XXXXX$$+x$$$XX$$$$XXX$X   :x$x+:  +xXXXx+.  
+;.            +XXXX$X: ;$$$XX.;$$$X$XX.     ;$$XXXXXXXXXXx:
               XXXX$$X. :$$$X:;$$$$$$$$.       x$XXXXXXXX$$;
              ;$$$$$$$X  +;::.$$$$$$$X;       .$$XXXXXX$$$+ 
               +$$$$$$;     .X$$$$X;.         $$$$$$$X$$$+  
                 +$$$:    +X$&&x.             x$$$$$X$$&;   
                  ;$$$.   :;$$$$X;           .  :$$$$$$:    
                .+XXXXX:::;+x$$XXX$X$XXxx++++++xxxxxx+;:.   
             ;x$XX$XXXX$$XXXXxxxxxxx++;:.                   
             ::;:;;;;:;;:......                             
";                    

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

        internal static void BeepOnWrongEntry()
        {
            Console.Beep(300, 60);
        }
    }    
}
