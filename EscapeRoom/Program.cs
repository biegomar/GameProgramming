namespace EscapeRoom
{
    internal class Program
    {      
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