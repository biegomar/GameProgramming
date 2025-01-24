// See https://aka.ms/new-console-template for more information

using CellularAutomata;
using GameOfLive;

var dimension = new Vector(100,40,0);
var screenSize = new Vector(dimension.X + 5, dimension.Y + 5, 0);
var playGround = new PlayGround<bool>(dimension);
var ruleSet = new GameOfLifeRuleSet();

GameOfLifeInitializer.Randomize(playGround, 0.15);

GameOfLifeInitializer.AddGlider(playGround, new Vector(20, 20, 0));
GameOfLifeInitializer.AddToad(playGround, new Vector(50, 20, 0));
GameOfLifeInitializer.AddBlinker(playGround, new Vector(10, 10, 0));

ConsoleVisualizer.SetConsoleSize(screenSize);

Console.Clear();
Console.CursorVisible = false;

do
{
    ConsoleVisualizer.Render(playGround, x => x ? 'X' : ' ');
    playGround = Automata<bool>.NextGeneration(playGround, ruleSet);
    Thread.Sleep(250);
    
    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
    {
        break; 
    }
} while (true);

Console.CursorVisible = true;