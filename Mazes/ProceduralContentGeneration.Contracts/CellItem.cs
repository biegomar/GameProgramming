namespace Mazes.Contracts
{
    public class CellItem<T>
    {
        public T Item { get;  }
        public CellVector Position { get;  }

        public CellItem(T item, CellVector position) 
        {
            this.Item = item;
            this.Position = position;
        }
    }    
}