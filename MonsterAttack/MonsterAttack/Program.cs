namespace MonsterAttack
{
    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {        
        /// <summary>
        /// The main entry point of the application.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            PrintIntro();

            GameLoop gameLoop = new GameLoop();           
            gameLoop.Run();
        }

        /// <summary>
        /// Print logo and rules.
        /// </summary>
        private static void PrintIntro()
        {
            Utils.PrintLogo();
            Utils.PrintRulez();
        }
    }
}