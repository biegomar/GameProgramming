namespace MonsterAttack
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            PrintIntro();

            GameLoop gameLoop = new GameLoop();           
            gameLoop.Run();
        }

        private static void PrintIntro()
        {
            Utils.PrintLogo();
            Utils.PrintRulez();
        }
    }
}