// See https://aka.ms/new-console-template for more information

using Dungeon.Cli;
using Mazes.Contracts;
using Mazes.Contracts.Cells;
using Mazes.Contracts.Dungeons;
using Mazes.Contracts.Printing;

//var dimension = new CellVector(Console.BufferWidth, Console.BufferHeight, 0);
var dimension = new CellVector(120, 30, 0);
var grid = new CellVector(3, 3, 0);
var properties = new DungeonProperties(dimension, grid, 4);
var proceduralContentGenerator = new DungeonOfDoomGenerator<char>(properties);
var contentPrinter = new SimpleDungeonPrinter<char>(properties);
var dungeon = GenerateMaze(proceduralContentGenerator, dimension, contentPrinter, CellVector.Zero, string.Empty);

Console.ReadKey();

static Landscape<char> GenerateMaze(IProceduralContentGenerator<char> generator, CellVector dimension, IContentPrinter<char> printer, CellVector screenPosition, string title)
{
    Console.Clear();
    
    var dungeon = new Landscape<char>(dimension, generator, printer, title);
    dungeon.Draw(screenPosition);

    return dungeon;
}