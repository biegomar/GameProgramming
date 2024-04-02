namespace Mazes.Contracts
{
    public class CellItem
    {
        public char Item { get;  }
        public MazeVector Position { get;  }

        public CellItem(char item, MazeVector position) 
        {
            this.Item = item;
            this.Position = position;
        }
    }    
}