using System.Diagnostics;

namespace MonsterAttack
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
            Utils.PrintLogo();
            Utils.PrintRulez();
        }
    }
}