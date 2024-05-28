// See https://aka.ms/new-console-template for more information

using Dungeon.Cli;
using Mazes.Contracts;
using Mazes.Contracts.Cells;
using Mazes.Contracts.Dungeons;

var dimension = new CellVector(81, 25, 0);
var grid = new CellVector(3, 3, 0);
var properties = new DungeonProperties(dimension, grid, 4);
var proceduralContentGenerator = new DungeonOfDoomGenerator<char>(properties);
var contentPrinter = new SimpleDungeonPrinter<char>();
var dungeon = new Landscape<char>(dimension, proceduralContentGenerator, contentPrinter);

dungeon.Draw(CellVector.Zero);

Console.ReadKey();