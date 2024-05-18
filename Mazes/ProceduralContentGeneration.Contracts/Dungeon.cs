using System.Collections.Generic;

namespace Mazes.Contracts
{
    public class Dungeon<T>
    {
        //public IList<Cell<T>> Cells { get; set; }
        public Cell<T>?[,] Cells { get; }
        
        public string? Title { get; }
        
        private readonly IProceduralContentGenerator<T> proceduralContentGenerator;
        private readonly IContentPrinter<T> dungeonPrinter;

        public Dungeon(IProceduralContentGenerator<T> proceduralContentGenerator, IContentPrinter<T> dungeonPrinter) 
            : this(proceduralContentGenerator, dungeonPrinter, string.Empty)
        {
                     
        }

        public Dungeon(IProceduralContentGenerator<T> proceduralContentGenerator, IContentPrinter<T> dungeonPrinter, string title)
        {
            //this.Cells = new List<Cell<T>>();
            this.proceduralContentGenerator = proceduralContentGenerator;
            this.dungeonPrinter = dungeonPrinter;
            this.Title = title;
        }
        
        public void Draw(MazeVector startMazeVector)
        {
            this.dungeonPrinter.DrawCells(this.Cells, startMazeVector, this.Title, false);
        }

        public void DrawCellItems()
        {
            this.dungeonPrinter.DrawCellItems(this.Cells);
        }

        public void DrawItemAtPosition(MazeVector position, T item)
        {
            this.dungeonPrinter.DrawItemAtPosition(this.Cells, position, item);
        }
    }
}