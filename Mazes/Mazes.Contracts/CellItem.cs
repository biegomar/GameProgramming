namespace Mazes.Contracts
{
    public class CellItem<T>
    {
        public T Item { get;  }
        public MazeVector Position { get;  }

        public CellItem(T item, MazeVector position) 
        {
            this.Item = item;
            this.Position = position;
        }
    }    
}