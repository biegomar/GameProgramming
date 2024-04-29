// See https://aka.ms/new-console-template for more information

using AldousBroderMaze;
using Mazes.Contracts;
using NearlyRogue.Cli;
using NearlyRogue.Core.Monsters;

Console.Clear();
            
var dimension = new MazeVector(10, 10, 0);
var maze = new Maze<Monster<char>>(dimension, new AldousBroderMazeGenerator<Monster<char>>(null),
    new ConsoleMazePrinter<Monster<char>>(), "AldousBroder");

var factory = new MonsterFactory<char>();
var monster = factory.CreateMonster(MonsterRace.Bat);
monster.Icon = '@';

maze.SetCellItem(new CellItem<Monster<char>>(monster, new MazeVector(1,1,0)));

maze.Draw(new MazeVector(0,0,0));

maze.DrawCellItems();

Console.ReadKey();