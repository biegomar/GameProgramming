// See https://aka.ms/new-console-template for more information

using CellularAutomata;
using GameOfLive;

var dimension = new Vector(100,40,0);
var screenSize = new Vector(dimension.X + 5, dimension.Y + 5, 0);
var playGround = new PlayGround<bool>(dimension);
var ruleSet = new GameOfLifeRuleSet(playGround);
var automata = new Automata<bool>(playGround, ruleSet);

GameOfLifeInitializer.Randomize(playGround, 0.15); // 30% der Zellen werden initial "lebendig" sein

ConsoleVisualizer.SetConsoleSize(screenSize);

Console.Clear();
Console.CursorVisible = false;

do
{
    automata.NextGeneration();
    ConsoleVisualizer.Render(playGround, x => x ? 'X' : ' ');
    Thread.Sleep(500);
    
    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
    {
        break; 
    }
} while (true);

Console.CursorVisible = true;