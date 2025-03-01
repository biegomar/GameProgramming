// See https://aka.ms/new-console-template for more information

using CellularAutomata;
using GameOfLive;

var dimension = new Vector(100,40);
var screenSize = new Vector(dimension.X + 5, dimension.Y + 5);
var playGround = new PlayGround<bool>(dimension);
var ruleSet = new GameOfLifeRuleSet();
Automata<bool> automataBool = new (dimension);

GameOfLifeInitializer.Randomize(playGround, 0.2);

//GameOfLifeInitializer.AddGlider(playGround, new Vector(4, 4, 0));
//GameOfLifeInitializer.AddToad(playGround, new Vector(50, 15, 0));
//GameOfLifeInitializer.AddBlinker(playGround, new Vector(40, 15, 0));
//GameOfLifeInitializer.AddBeacon(playGround, new Vector(70, 30, 0));

ConsoleVisualizer<bool>.SetConsoleSize(screenSize);


Console.Clear();
Console.CursorVisible = false;

do
{
    //ConsoleVisualizer.Render(playGround, x => x ? 'X' : ' ');
    ConsoleVisualizer<bool>.RenderWithColors(playGround, x => x ? ConsoleColor.Green : ConsoleColor.Black);
    playGround = automataBool.NextGeneration(playGround, ruleSet, false);
    Thread.Sleep(250);
    
    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
    {
        break; 
    }
} while (true);

Console.CursorVisible = true;