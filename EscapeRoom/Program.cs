using System.Runtime.CompilerServices;

namespace EscapeRoom
{
    internal class Program
    {      
        static void Main(string[] args)
        {
            PrintIntro();
                       

            GameLoop.Run();
        }      

        private static void PrintIntro() 
        { 
            Constants.PrintLogo();
        }

       
    }
}