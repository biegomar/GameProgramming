using System.Collections.Generic;
using Mazes.Contracts.Cells;

namespace Mazes.Contracts.Dungeons
{
    public class DungeonOfDoomGenerator<T> : BaseDungeonGenerator<T>
    {
        
        public override IList<Cell<T>> Generate(IList<Cell<T>> cells)
        {
            throw new System.NotImplementedException();
        }
    }
}