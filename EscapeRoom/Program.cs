namespace EscapeRoom
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

            GameLoop.Run();
        }      

        /// <summary>
        /// Give some advise.
        /// </summary>
        private static void PrintIntro() 
        { 
            Utils.PrintLogo();
            Utils.PrintRulez();
        }      
    }
}